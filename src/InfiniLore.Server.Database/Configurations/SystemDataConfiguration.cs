// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Database.Configurations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SystemDataConfiguration<TModel> : BasicDataConfiguration<TModel> where TModel : SystemData {
    public override void Configure(EntityTypeBuilder<TModel> builder) {
        base.Configure(builder);
    }
}
