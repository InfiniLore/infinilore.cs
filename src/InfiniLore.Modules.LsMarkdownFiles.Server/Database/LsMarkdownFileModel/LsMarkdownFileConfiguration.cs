// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.LsMarkdownFiles.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Modules.LsMarkdownFiles.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LsMarkdownFileConfiguration : OwnedModelConfiguration<LoreScopeModel, LsMarkdownFileModel> {
    public override void Configure(EntityTypeBuilder<LsMarkdownFileModel> builder) {
        base.Configure(builder);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(LsMarkdownFileModel.Defaults.NameMaxLength);
        
        builder.HasIndex(x => new { x.OwnerId, x.Name, x.SoftDeleteDate })
            .IsUnique()
            .HasDatabaseName("IX_OwnerId_Name_Unique");

        builder.HasOne(x => x.S3FileMetaData)
            .WithOne()
            .HasForeignKey<LsMarkdownFileModel>(x => x.S3FileMetaDataId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Ignore(x => x.ResourceUrl);
    }
}
