// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Modules.Core.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class KeyValueEntryConfiguration : IEntityTypeConfiguration<KeyValueEntryModel> {
    public void Configure(EntityTypeBuilder<KeyValueEntryModel> builder) {
        builder.HasKey(x => x.Key);
        builder.HasIndex(x => x.Key).IsUnique();

        builder.Property(x => x.Key)
            .HasMaxLength(KeyValueEntryModel.Defaults.KeyMaxLength)
            .IsRequired();

        builder.Property(x => x.Value)
            .HasMaxLength(KeyValueEntryModel.Defaults.ValueMaxLength);
    }
}
