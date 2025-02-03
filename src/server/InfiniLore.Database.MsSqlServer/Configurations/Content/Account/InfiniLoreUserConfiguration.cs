// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Account;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Database.MsSqlServer.Configurations.Content.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniLoreUserConfiguration : IEntityTypeConfiguration<InfiniLoreUser> {

    public void Configure(EntityTypeBuilder<InfiniLoreUser> builder) {
        #region LoreScopes
        builder.HasMany(user => user.Lorescopes)
            .WithOne(scope => scope.Owner)
            .HasForeignKey(x => x.OwnerId);

        builder.Property(u => u.LorescopeIds)
            .HasConversion(ConverterHelpers.GuidCollectionConverter)
            .Metadata.SetValueComparer(ConverterHelpers.GuidCollectionComparer);
        #endregion
        
        #region Multiverses
        builder.HasMany(user => user.Multiverses)
            .WithOne(multiverse => multiverse.Owner)
            .HasForeignKey(x => x.OwnerId);

        builder.Property(u => u.MultiverseIds)
            .HasConversion(ConverterHelpers.GuidCollectionConverter)
            .Metadata.SetValueComparer(ConverterHelpers.GuidCollectionComparer);
        #endregion

        #region Universes
        builder.HasMany(user => user.Universes)
            .WithOne(universe => universe.Owner)
            .HasForeignKey(x => x.OwnerId);
        
        builder.Property(u => u.UniverseIds)
            .HasConversion(ConverterHelpers.GuidCollectionConverter)
            .Metadata.SetValueComparer(ConverterHelpers.GuidCollectionComparer);
        #endregion

        builder.HasMany(user => user.JwtRefreshTokens)
            .WithOne(token => token.Owner)
            .HasForeignKey(token => token.OwnerId);

        builder.HasMany(user => user.ContentAccesses)
            .WithOne()
            .HasForeignKey(access => access.UserId);

        builder.HasMany(user => user.Permissions)
            .WithMany(permission => permission.Users);
    }
}
