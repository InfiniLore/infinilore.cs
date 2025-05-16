// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MarkdownDocumentConfiguration<TOwner, TModel> : OwnedModelConfiguration<TOwner, TModel> 
    where TModel : MarkdownDocumentModel<TOwner>
    where TOwner : BasicModel
{
    public override void Configure(EntityTypeBuilder<TModel> builder) {
        base.Configure(builder);
        
        builder.Property(x => x.Title)
            .HasMaxLength(MarkdownDocumentModel<TOwner>.Defaults.TitleMaxLength);
    }
}
