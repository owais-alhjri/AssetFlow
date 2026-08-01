using System.Text;
using AssetFlow.Application;
using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Infrastructure;
using AssetFlow.Infrastructure.Data;
using AssetFlow.Infrastructure.Data.Seeding;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpOverrides;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// ---- JWT validation (the second half of JWT — checks incoming tokens) ----
var jwtSecret = builder.Configuration["Jwt:Secret"]
    ?? throw new InvalidOperationException("Jwt:Secret is not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret))
        };
    });


builder.Services.AddAuthorization();

var allowedOrigins = builder.Configuration
.GetSection("Cors:AllowedOrigins")
.Get<string[]>() ?? [];
// ---- CORS for the Angular frontend ----
builder.Services.AddCors(options =>
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins(allowedOrigins)   // Angular dev server
              .AllowAnyMethod()
              .AllowAnyHeader()));

// ---- Swagger with a JWT "Authorize" button ----
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste your JWT here (no 'Bearer' prefix needed)."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                    { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
}
);

var app = builder.Build();

// ---- Seed on startup (before serving requests) ----
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var db = services.GetRequiredService<AssetFlowDbContext>();
        logger.LogInformation("Applying migrations...");
        await db.Database.MigrateAsync();

        var seeder = new DatabaseSeeder(db, services.GetRequiredService<IPasswordHasher>());
        await seeder.SeedAsync();
        logger.LogInformation("Database ready.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Database migration/seeding failed on startup.");
        throw;   // still crash — a half-migrated app is worse than a failed deploy
    }
}

// ---- Middleware pipeline — ORDER MATTERS ----
app.UseForwardedHeaders();   // must be first: later middleware reads the corrected scheme

// Swagger is public on purpose — this is a portfolio API and every
// endpoint is already [Authorize]-gated.
app.UseSwagger();
app.UseSwaggerUI();

// Only meaningful locally. Render terminates TLS at the edge, and leaving this
// on in production 307s the CORS preflight.
if (app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapControllers();

app.Run();