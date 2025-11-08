// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Core.Models;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AssetType {
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = null!;
    public string? SchemaJson { get; set; }
}

public class AssetTypeConfiguration : IEntityTypeConfiguration<AssetType> {
    public void Configure(EntityTypeBuilder<AssetType> builder) {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name).IsRequired();
    }
}

