// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Database.MsSqlServer.Configurations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class UserContentConfiguration<T> : BasicContentConfiguration<T> where T : UserContent {
    public override void Configure(EntityTypeBuilder<T> builder) {
        base.Configure(builder);// Call BasicContentConfiguration

        // Index on IsPublic and OwnerId to allow for fast filtering of public content by user
        builder.HasIndex(x => new {
            x.Id, x.OwnerId,
            IsPublic = x.IsPubliclyReadable
        });
    }
}
