// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Models.BaseModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Core.Models.Files;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LocalFileMetaDataModel : BaseModel {
    public required string FolderPath { get; set; }
    public required string FileName { get; set; }
    public required string ContentType { get; set; }
    
    public required long FileSize { get; set; }
    public required long FileHash { get; set; }
}

public class LocalFileMetaDataModelConfiguration : BaseModelConfiguration<LocalFileMetaDataModel> {
    public override void Configure(EntityTypeBuilder<LocalFileMetaDataModel> builder) {
        base.Configure(builder);
        
        builder.Property(model => model.FolderPath)
            .HasMaxLength(256)
            .IsRequired();
        
        builder.Property(model => model.FileName)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(model => model.ContentType)
            .HasMaxLength(256);
        
        builder.HasIndex(model => new {model.FolderPath, model.FileName}, "IX_LocalFile_FolderPath_FileName")
            .IsUnique();
    }
}
