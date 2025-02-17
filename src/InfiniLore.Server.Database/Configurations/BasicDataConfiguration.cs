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
// Not used by EFC to make the configuration for BasicData, see class above
public abstract class BasicDataConfiguration : IEntityTypeConfiguration<BasicData> {
    public void Configure(EntityTypeBuilder<BasicData> builder) {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
        
        builder.HasQueryFilter(model => model.SoftDeleteDate == null);
        
        builder.Ignore(x => x.IsSoftDeleted);
        
        builder.UseTptMappingStrategy();
    }
}