// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Modules.Core.Server.Database;
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
        
        builder.HasOne(x => x.Owner)
            .WithMany()
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.Property(x => x.OwnerId).IsRequired();
        builder.HasIndex(x => x.OwnerId);
    }
}
