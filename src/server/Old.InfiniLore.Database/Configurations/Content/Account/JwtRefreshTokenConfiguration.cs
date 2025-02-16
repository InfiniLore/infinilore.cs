// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Old.InfiniLore.Database.Models.Content.Account;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Old.InfiniLore.Database.Configurations.Content.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class JwtRefreshTokenConfiguration : IEntityTypeConfiguration<JwtRefreshTokenModel> {

    public void Configure(EntityTypeBuilder<JwtRefreshTokenModel> builder) {
        builder.HasIndex(token => token.TokenHash)
            .IsUnique();
    }
}
