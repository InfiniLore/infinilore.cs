// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;
using InfiniLore.Core.Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Modules.Users.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UserModel : BaseModel {
    public string UserName { get; set; } = string.Empty;
}

public class UserModelConfiguration : BaseModelConfiguration<UserModel> {
    public override void Configure(EntityTypeBuilder<UserModel> builder) {
        base.Configure(builder);
        
        builder.Property(model => model.UserName)
            .HasMaxLength(256)
            .IsRequired();
        
        builder.HasAlternateKey(model => model.UserName);
        builder.HasIndex(model => model.UserName)
            .IsUnique();
    }
}

[InjectableScoped<IValidator<UserModel>>]
public class UserModelValidator: BaseModelValidator<UserModel> {
    public UserModelValidator() {
        RuleFor(model => model.UserName)
            .NotEmpty()
            .NotNull()
            .MaximumLength(256)
            .Matches("^[a-zA-Z0-9_-]+$");
    }
}