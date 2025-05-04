// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class KeyValueEntryConfiguration : IEntityTypeConfiguration<KeyValueEntry> {
    public void Configure(EntityTypeBuilder<KeyValueEntry> builder) {
        builder.HasKey(x => x.Key);
        builder.HasIndex(x => x.Key).IsUnique();

        builder.Property(x => x.Key)
            .HasMaxLength(KeyValueEntry.Defaults.KeyMaxLength)
            .IsRequired();

        builder.Property(x => x.Value)
            .HasMaxLength(KeyValueEntry.Defaults.ValueMaxLength);
    }
}
