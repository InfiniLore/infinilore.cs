// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Models.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Core.Models.BaseModels;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BaseAssetModel : BaseOwnedModel<ProjectModel> {
    public required string Path { get; set; }
    public required string Name { get; set; }
}

public class BaseAssetModelConfiguration<TModel> : BaseOwnedModelConfiguration<TModel, ProjectModel> where TModel : BaseAssetModel {
    public override void Configure(EntityTypeBuilder<TModel> builder) {
        base.Configure(builder);
        
        builder.Property(model => model.Path)
            .HasMaxLength(1024)
            .IsRequired();
        
        builder.Property(model => model.Name)
            .HasMaxLength(256)
            .IsRequired();
        
        builder.HasIndex(model => new {model.OwnerId, model.Path, model.Name, model.SoftDeletedAt}, "IX_Asset_OwnerId_Path_Name_SoftDeletedAt")
            .IsUnique();
    }
}
