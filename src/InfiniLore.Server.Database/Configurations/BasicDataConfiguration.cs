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
public class BasicDataConfiguration : BasicDataConfiguration<BasicData> ;

// Not used by EFC to make the configuration for BasicData, see class above
public abstract class BasicDataConfiguration<T> : IEntityTypeConfiguration<T> where T : BasicData {
    public virtual void Configure(EntityTypeBuilder<T> builder) {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
        
        builder.HasQueryFilter(model => model.SoftDeleteDate == null);
    }
}