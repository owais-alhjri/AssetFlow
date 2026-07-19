using AssetFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetFlow.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.IsActive).IsRequired();

        // ---- Owned value objects ----
        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(x => x.Value)
                .HasColumnName("Email")
                .HasMaxLength(256)
                .IsRequired();

            email.HasIndex(x => x.Value).IsUnique();   // login identity — must be unique
        });

        builder.OwnsOne(u => u.PasswordHash, ph =>
        {
            ph.Property(x => x.Value)
                .HasColumnName("PasswordHash")
                .HasMaxLength(256)
                .IsRequired();
        });

        // ---- Relationships ----

        // Every user MUST have a role (non-nullable FK).
        builder.HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // A user MAY be linked to an employee (nullable FK).
        builder.HasOne(u => u.Employee)
            .WithOne()
            .HasForeignKey<User>(u => u.EmployeeId)
            .IsRequired(false)                        // ← the nullable relationship
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(u => u.EmployeeId)
            .IsUnique()
            .HasFilter("\"EmployeeId\" IS NOT NULL");// delete employee → user.EmployeeId becomes null

        builder.Property(u => u.CreatedAt)
            .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        builder.Property(u => u.UpdatedAt)
            .HasConversion(
                v => v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);
        builder.Property(u => u.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
    }
}