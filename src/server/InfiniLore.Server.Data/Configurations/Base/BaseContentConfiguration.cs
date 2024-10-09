// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Models.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Data.Configurations.Base;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BaseContentConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseContent<T> {
    public abstract void Configure(EntityTypeBuilder<T> builder);
    
    protected void HasSoftDeleteAsQueryFilter(EntityTypeBuilder<T> builder) {
        builder.HasQueryFilter(model => model.SoftDeleteDate == null);
    }

    protected void HasAuditLogs(EntityTypeBuilder<T> builder) {
        builder.HasMany<AuditLog<T>>(user => user.AuditLogs)
            .WithOne(scope => scope.Content)
            .HasForeignKey(x => x.ContentId);
    }

    protected void HasUniqueIdAsKey(EntityTypeBuilder<T> builder) {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
    }
}
