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
public class InfiniLoreUserConfiguration : IEntityTypeConfiguration<InfiniLoreUser>{

    public void Configure(EntityTypeBuilder<InfiniLoreUser> builder) {
        builder.HasKey(x => x.Id);
        
        builder.HasMany(x => x.JwtRefreshTokens)
            .WithOne(x => x.Owner)
            .HasForeignKey(x => x.OwnerId);
    }
}
