// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// Not used by EFC to make the configuration for BasicData, see class above
public abstract class OwnedDataConfiguration<TOwner, TModel> : IEntityTypeConfiguration<TModel> 
    where TModel : OwnedData<TOwner>
    where TOwner : class, IBasicData 
{
    public virtual void Configure(EntityTypeBuilder<TModel> builder) {
        builder.HasOne(x => x.Owner)
            .WithMany()
            .HasForeignKey(x => x.OwnerId)
            .IsRequired();

        builder.UseTptMappingStrategy();
    }
}
