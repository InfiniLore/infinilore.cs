// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Models.Account;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Data.Configurations.Account;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class JwtRefreshTokenConfiguration : IEntityTypeConfiguration<JwtRefreshToken> {

    public void Configure(EntityTypeBuilder<JwtRefreshToken> builder) {
        builder.HasIndex(token => token.TokenHash)
            .IsUnique();
    }
}
