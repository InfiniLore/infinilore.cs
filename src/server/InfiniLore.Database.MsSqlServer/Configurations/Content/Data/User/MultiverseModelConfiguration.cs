// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Data.User;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Database.MsSqlServer.Configurations.Content.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MultiverseModelConfiguration : UserContentConfiguration<MultiverseModel> {

    public override void Configure(EntityTypeBuilder<MultiverseModel> builder) {
        base.Configure(builder);

        builder.HasQueryFilter(model => model.SoftDeleteDate == null);

        builder.HasIndex(model => new { model.Name, model.LorescopeId }).IsUnique();

        builder.HasMany(model => model.Universes)
            .WithOne(universe => universe.Multiverse)
            .HasForeignKey(x => x.MultiverseId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
