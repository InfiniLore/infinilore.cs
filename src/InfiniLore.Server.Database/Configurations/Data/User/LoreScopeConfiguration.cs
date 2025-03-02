// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models.Data.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Database.Configurations.Data.User;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoreScopeConfiguration : UserDataConfiguration<LoreScope> {
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
