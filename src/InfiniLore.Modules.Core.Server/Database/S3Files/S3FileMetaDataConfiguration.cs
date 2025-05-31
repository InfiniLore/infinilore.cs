// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Modules.Core.Server.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class S3FileMetaDataConfiguration : BasicModelConfiguration<S3FileMetaDataModel> {
    public override void Configure(EntityTypeBuilder<S3FileMetaDataModel> builder) {
        base.Configure(builder);

        builder.Property(x => x.FileName)
            .IsRequired();
        
        builder.Property(x => x.ContentType)
            .IsRequired();

        builder.Ignore(x => x.FileStream);
        builder.Ignore(x => x.S3ResourceUrl);
    }
}
