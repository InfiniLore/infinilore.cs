// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;
using InfiniLore.Core.Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Core.Modules.Notes.Models;

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

[InjectableSingleton<IValidator<NoteAssetModel>>]
public class NoteAssetModelValidator : BaseAssetModelValidator<NoteAssetModel> {
    public NoteAssetModelValidator() {
        RuleFor(model => model.FileId)
            .NotEmpty();
    }
}
