// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Database;
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
        
        builder.HasIndex(x => new { x.OwnerId, x.Name, x.SoftDeleteDate })
            .IsUnique()
            .HasDatabaseName("IX_OwnerId_Name_Unique");

        builder.HasOne(x => x.AccessProtection)
            .WithOne()
            .HasForeignKey<LoreScopeModel>(x => x.AccessProtectionId)
            .IsRequired();
        
        builder.HasIndex(x => x.AccessProtectionId)
            .IsUnique()
            .HasDatabaseName("IX_LoreScopeModel_AccessProtectionId");
    }
}
