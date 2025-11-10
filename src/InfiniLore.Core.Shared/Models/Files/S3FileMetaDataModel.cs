// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Models.BaseModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Core.Models.Files;

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
