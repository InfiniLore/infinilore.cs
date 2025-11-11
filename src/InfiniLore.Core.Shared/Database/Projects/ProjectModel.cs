// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Core.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ProjectModel : BaseOwnedModel<UserModel> {
    public string Name { get; set; } = string.Empty;
}

public class ProjectModelConfiguration : BaseOwnedModelConfiguration<ProjectModel, UserModel> {
    public override void Configure(EntityTypeBuilder<ProjectModel> builder) {
        base.Configure(builder);
        
        builder.Property(model => model.Name)
            .HasMaxLength(256)
            .IsRequired();
        
        builder.HasIndex(model => new {model.OwnerId, model.Name}, "IX_Project_OwnerId_Name")
            .IsUnique();
    }
}

[InjectableScoped<IValidator<ProjectModel>>]
public class ProjectModelValidator : BaseOwnedModelValidator<ProjectModel, UserModel> {
    public ProjectModelValidator() {
        RuleFor(model => model.Name)
            .NotEmpty()
            .NotNull()
            .MaximumLength(256);
    }
}
