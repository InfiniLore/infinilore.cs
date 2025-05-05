// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Database;
using InfiniLore.Server.Modules.LoreScopes.Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Modules.MarkdownFiles.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MarkdownFileConfiguration : OwnedDataConfiguration<ILoreScope, MarkdownFile> {
    public override void Configure(EntityTypeBuilder<MarkdownFile> builder) {
        base.Configure(builder);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(MarkdownFile.Defaults.NameMaxLength);

        builder.Property(x => x.Source)
            .HasMaxLength(MarkdownFile.Defaults.SourceMaxLength);
    }
}
