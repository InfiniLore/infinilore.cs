// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FluentValidation;
using InfiniLore.Core.Database;
using InfiniLore.Modules.Projects.Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Modules.Assets.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract record BaseAssetModel : OwnedModel<ProjectModel> {
    public required string Path { get; set; }
    public required string Name { get; set; }
}

public class BaseAssetModelConfiguration<TModel> : OwnedModelConfiguration<TModel, ProjectModel>
    where TModel : BaseAssetModel {
    public override void Configure(EntityTypeBuilder<TModel> builder) {
        base.Configure(builder);

        builder.Property(model => model.Path)
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(model => model.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasIndex(model => new { model.OwnerId, model.Path, model.Name, model.SoftDeletedAt }, "IX_Asset_OwnerId_Path_Name_SoftDeletedAt")
            .IsUnique();
    }
}

public abstract class BaseAssetModelValidator<TModel> : OwnedModelValidator<TModel, ProjectModel>
    where TModel : BaseAssetModel {
    protected BaseAssetModelValidator() {
        RuleFor(model => model.Path)
            .NotEmpty()
            .NotNull();

        RuleFor(model => model.Name)
            .NotEmpty()
            .NotNull();
    }
}
