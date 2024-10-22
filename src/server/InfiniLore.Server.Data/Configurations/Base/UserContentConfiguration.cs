// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Models.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Data.Configurations.Base;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class UserContentConfiguration<T> : BaseContentConfiguration<T> where T : UserContent<T> {
    public override void Configure(EntityTypeBuilder<T> builder) {
        base.Configure(builder); // Call BaseContentConfiguration
        
        builder.HasMany(model => model.UserAccess)
            .WithOne();

        // Index on IsPublic and OwnerId to allow for fast filtering of public content by user
        builder.HasIndex(x => new { x.Id, x.OwnerId, x.IsPublic });
    }
}
