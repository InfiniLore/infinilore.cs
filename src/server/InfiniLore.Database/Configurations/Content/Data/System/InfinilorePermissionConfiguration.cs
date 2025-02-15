// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Data.System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Database.Configurations.Content.Data.System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfinilorePermissionConfiguration : BasicContentConfiguration<InfiniLorePermission> {
    public override void Configure(EntityTypeBuilder<InfiniLorePermission> builder) {
        base.Configure(builder);

        builder.HasIndex(permission => permission.Name).IsUnique();
        builder.Property(permission => permission.Name).IsRequired().HasMaxLength(255);
        builder.HasIndex(permission => permission.NormalizedName).IsUnique();
        builder.Property(permission => permission.Name).HasMaxLength(255);
        builder.Property(permission => permission.Description).IsRequired().HasMaxLength(511).HasDefaultValue(string.Empty);
    }
}
