// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Modules.Core.Server.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AccessProtectionRuleModelConfiguration : OwnedModelConfiguration<AccessProtectionModel, AccessProtectionRuleModel> {
    public override void Configure(EntityTypeBuilder<AccessProtectionRuleModel> builder) {
        base.Configure(builder);
        
        builder.Property(x => x.UserId).IsRequired();
        builder.HasIndex(x => new { x.UserId, x.Permission })
            .IsUnique()
            .HasDatabaseName("IX_UserId_Permission_Unique");
        
        builder.Property(x => x.Permission)
            .IsRequired()
            .HasMaxLength(AccessProtectionRuleModel.Defaults.PermissionMaxLength);
    }
}
