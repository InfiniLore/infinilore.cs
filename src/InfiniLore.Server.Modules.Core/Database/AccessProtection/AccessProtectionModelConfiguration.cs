// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Modules.Core.Database;
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
            .HasForeignKey(x => x.ModelOwnerId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasMany(x => x.Rules);

        builder.Ignore(x => x.UserMappedRules);
    }
}
