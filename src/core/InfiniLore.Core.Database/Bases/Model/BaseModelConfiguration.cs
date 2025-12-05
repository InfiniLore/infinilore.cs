// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BaseModelConfiguration<TModel> : IEntityTypeConfiguration<TModel> where TModel : BaseModel {
    public virtual void Configure(EntityTypeBuilder<TModel> builder) {
        builder.HasKey(model => model.Id);
        builder.HasIndex(model => model.Id)
            .IsUnique();
        
        builder.Property(model => model.CreatedAt)
            .IsRequired();
        
        builder.Property(model => model.ModifiedAt)
            .IsRequired();
        
        builder.Ignore(model => model.IsSoftDeleted);
        builder.Property(model => model.SoftDeletedAt)
            .IsRequired();
        
        builder.HasQueryFilter(model => model.SoftDeletedAt == DateTime.MinValue);
        
        builder.Property(model => model.RowVersion)
            .IsRowVersion()
            .IsConcurrencyToken();

        builder.UseTptMappingStrategy();
    }
}
