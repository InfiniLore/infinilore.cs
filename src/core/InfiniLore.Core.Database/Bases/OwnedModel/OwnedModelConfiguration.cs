// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class OwnedModelConfiguration<TModel, TOwner> : BaseModelConfiguration<TModel> 
    where TModel: OwnedModel<TOwner>
    where TOwner : BaseModel {
    
    public override void Configure(EntityTypeBuilder<TModel> builder) {
        base.Configure(builder);
        
        builder.HasOne(model => model.Owner)
            .WithMany()
            .HasForeignKey(model => model.OwnerId);
        
        builder.HasIndex(model => model.OwnerId);
        builder.Property(model => model.OwnerId)
            .IsRequired();   
    }
}
