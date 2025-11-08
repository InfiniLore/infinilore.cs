// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Core.Models;
using InfiniLore.Core.Models.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LimitedNoteModel {
    public string? Title { get; set; }
    public Guid Author { get; set; } = Guid.Empty;
    public string? Data { get; set; }
}

public class LimitedNoteAssetModel : AssetModel<LimitedNoteModel> {
    public string? Title => Data.Title;
    public Guid Author => Data.Author;
    public string? NoteData => Data.Data;
}

public class LimitedNoteAssetModelConfiguration : IEntityTypeConfiguration<LimitedNoteAssetModel> {
    public void Configure(EntityTypeBuilder<LimitedNoteAssetModel> builder) {

        builder.Ignore(a => a.Title);
        builder.Ignore(a => a.Author);
        builder.Ignore(a => a.NoteData);
    }
}
