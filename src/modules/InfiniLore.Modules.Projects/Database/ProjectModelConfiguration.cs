// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using InfiniLore.Modules.Users.Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Modules.Projects.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ProjectModelConfiguration : OwnedModelConfiguration<ProjectModel, UserModel> {
    public override void Configure(EntityTypeBuilder<ProjectModel> builder) {
        base.Configure(builder);
        
        builder.Property(model => model.Name)
            .HasMaxLength(256)
            .IsRequired();
        
        builder.HasIndex(model => new {model.OwnerId, model.Name}, "IX_Project_OwnerId_Name")
            .IsUnique();
    }
}
