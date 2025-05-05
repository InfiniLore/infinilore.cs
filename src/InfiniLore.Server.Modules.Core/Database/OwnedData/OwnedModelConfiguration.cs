// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// Not used by EFC to make the configuration for BasicData, see class above
public abstract class OwnedModelConfiguration<TOwner, TModel> : BasicModelConfiguration<TModel> 
    where TModel : OwnedModel<TOwner>
    where TOwner : BasicModel 
{
    public override void Configure(EntityTypeBuilder<TModel> builder) {
        base.Configure(builder);
        builder.Property(x => x.OwnerId).IsRequired();
        builder.HasIndex(x => x.OwnerId);

        builder.Ignore(x => x.Owner);
    }
}
