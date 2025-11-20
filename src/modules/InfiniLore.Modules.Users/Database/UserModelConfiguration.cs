// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Modules.Users.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UserModelConfiguration : BaseModelConfiguration<UserModel> {
    public override void Configure(EntityTypeBuilder<UserModel> builder) {
        base.Configure(builder);
        
        builder.Property(model => model.UserName)
            .HasMaxLength(256)
            .IsRequired();
        
        builder.HasAlternateKey(model => model.UserName);
        builder.HasIndex(model => model.UserName)
            .IsUnique();
    }
}
