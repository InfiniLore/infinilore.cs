// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Models.Account;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Data.Configurations.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniLoreUserConfiguration : IEntityTypeConfiguration<InfiniLoreUser> {

    public void Configure(EntityTypeBuilder<InfiniLoreUser> builder) {
        builder.HasMany(user => user.LoreScopes)
            .WithOne(scope => scope.User)
            .HasForeignKey(x => x.UserId);

        builder.HasMany(user => user.Multiverses)
            .WithOne(multiverse => multiverse.User)
            .HasForeignKey(x => x.UserId);

        builder.HasMany(user => user.Universes)
            .WithOne(universe => universe.User)
            .HasForeignKey(x => x.UserId);

        builder.HasMany(user => user.JwtRefreshTokens)
            .WithOne(token => token.User)
            .HasForeignKey(token => token.UserId);
    }
}
