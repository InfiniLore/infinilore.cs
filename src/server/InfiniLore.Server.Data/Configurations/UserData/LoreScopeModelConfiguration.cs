// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Configurations.Base;
using InfiniLore.Server.Data.Models.UserData;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Data.Configurations.UserData;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoreScopeModelConfiguration : BaseContentConfiguration<LoreScopeModel> {
    public override void Configure(EntityTypeBuilder<LoreScopeModel> builder) {
        HasSoftDeleteAsQueryFilter(builder);
        HasUniqueIdAsKey(builder);
        HasAuditLogs(builder);
        
        builder.HasIndex(model => new { model.Name, model.UserId })
            .IsUnique();
        
        builder.HasMany(model => model.Multiverses)
            .WithOne(multiverse => multiverse.LoreScope)
            .HasForeignKey(x => x.LoreScopeId);
    }
}
