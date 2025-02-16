// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Old.InfiniLore.Database.Models.Content.Data.System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Old.InfiniLore.Database.Configurations.Content.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SystemInformationConfiguration : IEntityTypeConfiguration<SystemInformation>{

    public void Configure(EntityTypeBuilder<SystemInformation> builder) {
        builder.HasKey(info => info.Name);
        builder.HasIndex(info => info.Name)
            .IsUnique();
        
        builder.Property(info => info.Value).HasMaxLength(1024);
        builder.Property(info => info.Name).HasMaxLength(256);
    }
}
