using AssetFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetFlow.Infrastructure.Data.Configurations;

public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.ToTable("Assignments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.AssignedDate).IsRequired();
        builder.Property(a => a.ReturnedDate);                 // nullable — still out until returned

        // Both AssetCondition enums → strings.
        builder.Property(a => a.ConditionAtAssign)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(a => a.ConditionAtReturn)
            .HasConversion<string>()
            .HasMaxLength(20);                                 // nullable — only set on return

        builder.Property(a => a.Notes).HasMaxLength(1000);
        builder.Property(a => a.ReturnNotes).HasMaxLength(1000);

        // ---- Relationships ----
        builder.HasOne(a => a.Asset)
            .WithMany()
            .HasForeignKey(a => a.AssetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Employee)
            .WithMany()
            .HasForeignKey(a => a.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(a => a.CreatedAt)
            .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        builder.Property(a => a.UpdatedAt)
            .HasConversion(
                v => v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);
    }
}