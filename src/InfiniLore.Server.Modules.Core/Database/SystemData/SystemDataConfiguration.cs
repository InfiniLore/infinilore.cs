// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SystemDataConfiguration<TModel> : BasicDataConfiguration<TModel> where TModel : SystemData {
    public override void Configure(EntityTypeBuilder<TModel> builder) {
        base.Configure(builder);
    }
}
