// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Database;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.MarkdownFiles.DataBase;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Modules.MarkdownFiles.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MarkdownFileConfiguration : OwnedModelConfiguration<LoreScopeModel, MarkdownFileModel> {
    public override void Configure(EntityTypeBuilder<MarkdownFileModel> builder) {
        base.Configure(builder);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(MarkdownFileModel.Defaults.NameMaxLength);

        builder.Property(x => x.Source)
            .HasMaxLength(MarkdownFileModel.Defaults.SourceMaxLength);
    }
}
