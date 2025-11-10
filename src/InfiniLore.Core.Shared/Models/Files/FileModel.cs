// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;
using InfiniLore.Core.Models.BaseModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Core.Models.Files;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class FileModel : BaseModel {
    public Guid S3FileMetaDataId { get; set; } = Guid.Empty;
    public S3FileMetaDataModel? S3FileMetaData {
        get;
        set {
            S3FileMetaDataId = value?.Id ?? Guid.Empty;
            field = value;       
        }
    } = null;

    public Guid LocalFileMetaDataId { get; set; } = Guid.Empty;
    public LocalFileMetaDataModel? LocalFileMetaData {
        get;
        set {
            LocalFileMetaDataId = value?.Id ?? Guid.Empty;
            field = value;
        }
    } = null;
}

public class FileModelConfiguration : BaseModelConfiguration<FileModel> {
    public override void Configure(EntityTypeBuilder<FileModel> builder) {
        base.Configure(builder);
        
        builder.Property(model => model.S3FileMetaDataId)
            .IsRequired();
        
        builder.HasOne(model => model.S3FileMetaData)
            .WithOne()
            .HasForeignKey<FileModel>(model => model.S3FileMetaDataId);
        
        builder.Property(model => model.LocalFileMetaDataId)
            .IsRequired();
        
        builder.HasOne(model => model.LocalFileMetaData)
            .WithOne()
            .HasForeignKey<FileModel>(model => model.LocalFileMetaDataId);
    }
}

[InjectableScoped<IValidator<FileModel>>]
public class FileModelValidator : BaseModelValidator<FileModel> {
    public FileModelValidator() {
        RuleFor(model => model.S3FileMetaDataId)
            .NotEmpty()
            .NotNull();
        
        RuleFor(model => model.LocalFileMetaDataId)
            .NotEmpty()
            .NotNull();
    }
}
