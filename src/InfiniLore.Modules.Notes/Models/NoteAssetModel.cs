// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Models.BaseModels;
using InfiniLore.Core.Models.Files;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Modules.Notes.Models;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class NoteAssetModel : BaseAssetModel {
    public Guid FileId { get; set; } = Guid.Empty;
    public FileModel? File { get; set; }
}

public class NoteAssetModelConfiguration : BaseAssetModelConfiguration<NoteAssetModel> {
    public override void Configure(EntityTypeBuilder<NoteAssetModel> builder) {
        base.Configure(builder);
        
        builder.Property(model => model.FileId)
            .IsRequired();
        
        builder.HasOne(model => model.File)
            .WithOne()
            .HasForeignKey<NoteAssetModel>(model => model.FileId);
    }
}
