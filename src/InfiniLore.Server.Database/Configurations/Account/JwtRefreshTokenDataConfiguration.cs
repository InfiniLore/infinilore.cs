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
        
        builder.Property(e => e.Roles)
            .HasConversion(
                v => string.Join(',', v),                
                v => v.Split(',', StringSplitOptions.None)
            );

        builder.Property(e => e.Permissions)
            .HasConversion(
                v => string.Join(',', v), 
                v => v.Split(',', StringSplitOptions.None)
            );

        builder.Ignore(x => x.ExpiresAt);
    }
}