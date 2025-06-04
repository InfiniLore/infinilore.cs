// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Modules.Core.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AccessProtectionModelConfiguration : BasicModelConfiguration<AccessProtectionModel> {
    public override void Configure(EntityTypeBuilder<AccessProtectionModel> builder) {
        base.Configure(builder);
        
        builder.HasIndex(x => x.ProtectedModelId).IsUnique();
        builder.Property(x => x.ProtectedModelId).IsRequired();
        
        builder.HasOne(x => x.ModelOwner)
            .WithMany()
            .HasForeignKey(x => x.ModelOwnerId);
        
        builder.HasMany(x => x.Rules);

        builder.Ignore(x => x.UserMappedRules);
    }
}
