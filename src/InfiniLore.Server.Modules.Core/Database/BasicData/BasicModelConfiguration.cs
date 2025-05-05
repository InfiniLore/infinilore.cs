// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// Not used by EFC to make the configuration for BasicData, see class above
public abstract class BasicModelConfiguration<TModel> : IEntityTypeConfiguration<TModel> where TModel : BasicModel {
    public virtual void Configure(EntityTypeBuilder<TModel> builder) {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();

        builder.HasQueryFilter(model => model.SoftDeleteDate == null);

        builder.Ignore(x => x.IsSoftDeleted);
        
        builder.UseTptMappingStrategy();
    }
}
