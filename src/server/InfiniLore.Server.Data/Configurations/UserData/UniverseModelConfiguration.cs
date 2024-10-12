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
public class UniverseModelConfiguration : BaseContentConfiguration<UniverseModel> {

    public override void Configure(EntityTypeBuilder<UniverseModel> builder) {
        HasSoftDeleteAsQueryFilter(builder);
        HasUniqueIdAsKey(builder);
        HasAuditLogs(builder);

        builder.HasQueryFilter(model => model.SoftDeleteDate == null);

        builder.HasIndex(model => new { model.Name, model.MultiverseId })
            .IsUnique();
    }
}
