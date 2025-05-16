// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MarkdownDocumentConfiguration : BasicModelConfiguration<MarkdownDocumentModel> {
    public override void Configure(EntityTypeBuilder<MarkdownDocumentModel> builder) {
        base.Configure(builder);
    }
}
