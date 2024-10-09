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
public class MultiverseModelConfiguration : BaseContentConfiguration<MultiverseModel> {

    public override void Configure(EntityTypeBuilder<MultiverseModel> builder) {
        HasSoftDeleteAsQueryFilter(builder);
        HasUniqueIdAsKey(builder);
        HasAuditLogs(builder);
        
        builder.HasQueryFilter(model => model.SoftDeleteDate == null);
        
        builder.HasIndex(model => new { model.Name, model.LoreScopeId }).
            IsUnique();
        
        builder.HasMany(model => model.Universes)
            .WithOne(universe => universe.Multiverse)
            .HasForeignKey(x => x.MultiverseId);
        
    }
}
