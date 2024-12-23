// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Database.MsSqlServer.Configurations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BasicContentConfiguration<T> : IEntityTypeConfiguration<T> where T : BasicContent {
    public virtual void Configure(EntityTypeBuilder<T> builder) {
        HasSoftDeleteAsQueryFilter(builder);
        HasUniqueIdAsKey(builder);
    }

    private static void HasSoftDeleteAsQueryFilter(EntityTypeBuilder<T> builder) {
        builder.HasQueryFilter(model => model.SoftDeleteDate == null);
    }

    private static void HasUniqueIdAsKey(EntityTypeBuilder<T> builder) {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
    }
}
