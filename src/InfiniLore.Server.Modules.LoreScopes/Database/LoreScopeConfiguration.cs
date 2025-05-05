// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Database;
using InfiniLore.Server.Modules.Core.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Modules.LoreScopes.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoreScopeConfiguration : OwnedDataConfiguration<IInfiniLoreUser, LoreScope> {
    public override void Configure(EntityTypeBuilder<LoreScope> builder) {
        base.Configure(builder);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(LoreScope.Defaults.NameMaxLength);

        builder.Property(x => x.ShortDescription)
            .HasMaxLength(LoreScope.Defaults.ShortDescriptionMaxLength);

        builder.HasIndex(x => new { x.OwnerId, x.Name })
            .IsUnique()
            .HasDatabaseName("IX_OwnerId_Name_Unique");

    }
}
