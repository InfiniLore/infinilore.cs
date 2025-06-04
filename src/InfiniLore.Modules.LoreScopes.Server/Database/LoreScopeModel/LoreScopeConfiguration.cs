// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Server.Modules.LoreScopes.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Modules.LoreScopes.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoreScopeConfiguration : OwnedModelConfiguration<InfiniLoreUserModel, LoreScopeModel> {
    public override void Configure(EntityTypeBuilder<LoreScopeModel> builder) {
        base.Configure(builder);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(LoreScopeModel.Defaults.NameMaxLength);
        
        builder.HasIndex(x => new { x.OwnerId, x.Name, x.SoftDeleteDate })
            .IsUnique()
            .HasDatabaseName("IX_OwnerId_Name_Unique");
        
        builder.HasOne(x => x.AccessProtection)
            .WithOne()
            .HasForeignKey<LoreScopeModel>(x => x.AccessProtectionId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(x => x.PosterImageMetaData)
            .WithOne()
            .HasForeignKey<LoreScopeModel>(x => x.PosterImageMetaDataId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.Ignore(x => x.HasAccessProtection);
    }
}
