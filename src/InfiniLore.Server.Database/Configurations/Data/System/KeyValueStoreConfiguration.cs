// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models.Data.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Database.Configurations.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class KeyValueStoreConfiguration : IEntityTypeConfiguration<KeyValueStore> {

    public void Configure(EntityTypeBuilder<KeyValueStore> builder) {
        builder.Property(x => x.Key)
            .HasMaxLength(KeyValueStore.Defaults.KeyMaxLength)
            .IsRequired();
        
        builder.Property(x => x.Value)
            .HasMaxLength(KeyValueStore.Defaults.ValueMaxLength);
    }
}
