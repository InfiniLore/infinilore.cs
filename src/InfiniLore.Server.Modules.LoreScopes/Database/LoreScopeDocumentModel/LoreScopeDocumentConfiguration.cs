// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Modules.LoreScopes.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoreScopeDocumentConfiguration : OwnedModelConfiguration<LoreScopeModel, LoreScopeDocumentModel> {
    public override void Configure(EntityTypeBuilder<LoreScopeDocumentModel> builder) {
        base.Configure(builder);

        builder.Property(x => x.Content)
            .IsRequired()
            .HasMaxLength(LoreScopeModel.Defaults.NameMaxLength);

        builder.Property(x => x.HtmlRenderedContent)
            .IsRequired(false);

        // Configure the one-to-one relationship with LoreScopeModel
        builder.HasOne(x => x.Owner)
            .WithOne(x => x.Document)
            .HasForeignKey<LoreScopeModel>(x => x.DocumentId);

    }
}
