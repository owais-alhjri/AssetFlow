using AssetFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetFlow.Infrastructure.Data.Configurations;

public class AssetStatusConfiguration : IEntityTypeConfiguration<AssetStatus>
{
    public void Configure(EntityTypeBuilder<AssetStatus> builder)
    {
        builder.ToTable("AssetStatuses");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(s => s.Name).IsUnique();   // GetIdByNameAsync relies on Name being unique

        builder.Property(s => s.IsActive)
            .IsRequired();
    }
}