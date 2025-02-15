// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Data.User;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Database.Configurations.Content.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LorescopeModelConfiguration : UserContentConfiguration<LorescopeModel> {

    public override void Configure(EntityTypeBuilder<LorescopeModel> builder) {
        base.Configure(builder);

        builder.HasIndex(model => new { model.Name, model.OwnerId })
            .IsUnique();
    }
}
