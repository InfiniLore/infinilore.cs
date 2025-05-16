// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Database;
using InfiniLore.Server.Modules.Users.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Modules.LoreScopes.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoreScopeConfiguration : OwnedModelConfiguration<InfiniLoreUserModel, LoreScopeModel> {
    public override void Configure(EntityTypeBuilder<LoreScopeModel> builder) {
        base.Configure(builder);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(LoreScopeModel.Defaults.NameMaxLength);
        
        builder.HasIndex(x => new { x.OwnerId, x.Name })
            .IsUnique()
            .HasDatabaseName("IX_OwnerId_Name_Unique"); 

        builder.Property(x => x.DescriptionId)
            .IsRequired(false);
    }
}
