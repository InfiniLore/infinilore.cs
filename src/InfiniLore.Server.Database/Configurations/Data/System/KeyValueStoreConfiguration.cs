// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models.Data.System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Database.Configurations.Data.System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class KeyValueStoreConfiguration : SystemDataConfiguration<KeyValueStore> {
    public override void Configure(EntityTypeBuilder<KeyValueStore> builder) {
        base.Configure(builder);
        
        builder.Property(x => x.Key)
            .HasMaxLength(KeyValueStore.Defaults.KeyMaxLength)
            .IsRequired();

        builder.Property(x => x.Value)
            .HasMaxLength(KeyValueStore.Defaults.ValueMaxLength);
    }
}
