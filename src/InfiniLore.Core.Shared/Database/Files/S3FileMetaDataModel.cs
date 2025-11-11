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
public class S3FileMetaDataModel : BaseModel {
    public required string BucketName { get; set; }
    public required string FileName { get; set; }
    public required string ContentType { get; set; }
}

public class S3FileMetaDataModelConfiguration : BaseModelConfiguration<S3FileMetaDataModel> {
    public override void Configure(EntityTypeBuilder<S3FileMetaDataModel> builder) {
        base.Configure(builder);
        
        builder.Property(model => model.BucketName)
            .HasMaxLength(256)
            .IsRequired();
        
        builder.Property(model => model.FileName)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(model => model.ContentType)
            .HasMaxLength(256);
        
        builder.HasIndex(model => new {model.BucketName, model.FileName}, "IX_S3File_BucketName_FileName")
            .IsUnique();
    }
    
}

[InjectableScoped<IValidator<S3FileMetaDataModel>>]
public class S3FileMetaDataModelValidator : BaseModelValidator<S3FileMetaDataModel> {
    public S3FileMetaDataModelValidator() {
        RuleFor(model => model.BucketName)
            .NotEmpty()
            .NotNull()
            .MaximumLength(256);

        RuleFor(model => model.FileName)
            .NotEmpty()
            .NotNull()
            .MaximumLength(256);

        RuleFor(model => model.ContentType)
            .NotEmpty()
            .NotNull()
            .MaximumLength(256);
    }
}