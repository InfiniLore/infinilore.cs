// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;

namespace InfiniLore.Modules.Core.Server.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<IValidator<S3FileMetaDataModel>>]
public class S3FileMetaDataValidator : BasicModelValidator<S3FileMetaDataModel> {
    public S3FileMetaDataValidator() {
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("The FileName field is required.")
            .MaximumLength(S3FileMetaDataModel.Defaults.MaxFileNameLength).WithMessage($"The {nameof(S3FileMetaDataModel.FileName)} cannot exceed {S3FileMetaDataModel.Defaults.MaxFileNameLength} characters.");
        
        RuleFor(x => x.ContentType)
            .NotEmpty().WithMessage("The ContentType field is required.")
            .MaximumLength(S3FileMetaDataModel.Defaults.MaxContentTypeLength).WithMessage($"The {nameof(S3FileMetaDataModel.ContentType)} cannot exceed {S3FileMetaDataModel.Defaults.MaxContentTypeLength} characters.");
    }
}
