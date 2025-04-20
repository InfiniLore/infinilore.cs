// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models.Data.Project;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Database.Configurations.Data.Project;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MarkdownFileConfiguration : ProjectDataConfiguration<MarkdownFile> {
    public override void Configure(EntityTypeBuilder<MarkdownFile> builder) {
        base.Configure(builder);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(MarkdownFile.Defaults.NameMaxLength);

        builder.Property(x => x.Source)
            .HasMaxLength(MarkdownFile.Defaults.SourceMaxLength);
    }
}
