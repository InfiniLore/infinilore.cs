// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Database.Configurations.Account;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class JwtRefreshTokenDataConfiguration : IEntityTypeConfiguration<JwtRefreshTokenData>{

    public void Configure(EntityTypeBuilder<JwtRefreshTokenData> builder) {
        builder.Property(x => x.TokenHash)
            .HasMaxLength(JwtRefreshTokenData.MaxLengthTokenHash)
            .IsRequired();
    }
}