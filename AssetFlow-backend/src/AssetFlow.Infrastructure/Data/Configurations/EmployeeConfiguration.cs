using AssetFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetFlow.Infrastructure.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.EmployeeNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(e => e.EmployeeNumber).IsUnique();   // employee numbers are identifiers

        builder.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
        builder.Property(e => e.LastName).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Department).HasMaxLength(50).IsRequired();
        builder.Property(e => e.JobTitle).HasMaxLength(100);
        builder.Property(e => e.IsActive).IsRequired();

        builder.OwnsOne(e => e.Email, email =>
        {
            email.Property(x => x.Value)
                .HasColumnName("Email")
                .HasMaxLength(256)
                .IsRequired();

            email.HasIndex(x => x.Value).IsUnique();   // no two employees share an email
        });

        builder.Property(e => e.CreatedAt)
            .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        builder.Property(e => e.UpdatedAt)
            .HasConversion(
                v => v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);
    }
}