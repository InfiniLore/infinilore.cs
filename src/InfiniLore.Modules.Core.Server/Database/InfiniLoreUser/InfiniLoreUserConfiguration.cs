// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Modules.Core.Server.Database.InfiniLoreUser;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniLoreUserConfiguration : BasicModelConfiguration<InfiniLoreUserModel> {

    public override void Configure(EntityTypeBuilder<InfiniLoreUserModel> builder) {
        base.Configure(builder);

        builder.HasIndex(x => x.Auth0IdGoogle).IsUnique();
        builder.Property(x => x.Auth0IdGoogle).HasMaxLength(InfiniLoreUserModel.Defaults.Auth0IdGoogleMaxLength);

        builder.HasIndex(x => x.Auth0Github).IsUnique();
        builder.Property(x => x.Auth0Github).HasMaxLength(InfiniLoreUserModel.Defaults.Auth0IdGithubMaxLength);

        builder.HasIndex(x => x.Auth0MailPassword).IsUnique();
        builder.Property(x => x.Auth0MailPassword).HasMaxLength(InfiniLoreUserModel.Defaults.Auth0MailPasswordMaxLength);

        builder.HasIndex(x => x.Username).IsUnique();
        builder.Property(x => x.Username).HasMaxLength(InfiniLoreUserModel.Defaults.UsernameMaxLength);

        builder.Ignore(x => x.Auth0Id);
        builder.Ignore(x => x.ProfileImageUrl);
        
        builder.HasOne(x => x.ProfileImageMetaData)
            .WithOne()
            .HasForeignKey<InfiniLoreUserModel>(x => x.ProfileImageMetaDataId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
