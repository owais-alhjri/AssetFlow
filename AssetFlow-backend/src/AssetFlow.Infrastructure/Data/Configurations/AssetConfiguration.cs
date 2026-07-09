using AssetFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetFlow.Infrastructure.Data.Configurations;

public class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.ToTable("Assets");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).IsRequired();

        // ---- Value objects (owned → flattened into columns on Assets) ----

        builder.OwnsOne(a => a.Tag, tag =>
        {
            tag.Property(t => t.Value)
                .HasColumnName("Tag")
                .HasMaxLength(50)
                .IsRequired();

            // Tag must be unique across assets — index lives on the owned property.
            tag.HasIndex(t => t.Value).IsUnique();
        });

        builder.OwnsOne(a => a.SerialNumber, sn =>
        {
            sn.Property(s => s.Value)
                .HasColumnName("SerialNumber")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.OwnsOne(a => a.PurchasePrice, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("PurchasePrice")
                .HasColumnType("decimal(18,2)")   // money → fixed precision, never float
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        // ---- Simple properties ----

        builder.Property(a => a.Name)
            .HasMaxLength(200)
            .IsRequired();

        // Enum stored as its string name ("Good"), not an int — readable in the DB.
        builder.Property(a => a.Condition)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(a => a.Location).HasMaxLength(200);
        builder.Property(a => a.Notes).HasMaxLength(1000);

        // ---- Relationships to OTHER entities (FK, not ownership) ----

        builder.HasOne(a => a.Category)
            .WithMany()
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Status)
            .WithMany()
            .HasForeignKey(a => a.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        // ---- UTC timestamps ----
        // Postgres hands back DateTimes as Unspecified kind; force UTC on read
        // so the rest of the app always sees Kind=Utc.
        builder.Property(a => a.CreatedAt)
            .HasConversion(
                v => v,
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        builder.Property(a => a.UpdatedAt)
            .HasConversion(
                v => v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

        // ---- Soft delete ----
        // Every query against Assets automatically excludes IsDeleted rows.
        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}