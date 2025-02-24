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
        builder.HasIndex(x => x.Auth0IdGoogle).IsUnique();
        builder.Property(x => x.Auth0IdGoogle).HasMaxLength(256);
        
        builder.HasIndex(x => x.Auth0Github).IsUnique();
        builder.Property(x => x.Auth0Github).HasMaxLength(256);
    }
}
