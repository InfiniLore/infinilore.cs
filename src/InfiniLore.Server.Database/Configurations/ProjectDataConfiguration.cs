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
public class ProjectDataConfiguration<TModel> : IEntityTypeConfiguration<TModel> where TModel : ProjectData {
    public virtual void Configure(EntityTypeBuilder<TModel> builder) {
        builder.HasOne(x => x.LoreScope)
            .WithMany()
            .HasForeignKey(x => x.LoreScopeId)
            .IsRequired();

        builder.UseTptMappingStrategy();
    }
    
}
