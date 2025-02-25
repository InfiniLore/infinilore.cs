// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Database.Configurations;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SystemDataConfiguration : IEntityTypeConfiguration<SystemData> {
    public void Configure(EntityTypeBuilder<SystemData> builder) {
        builder.UseTptMappingStrategy();
    }
}
