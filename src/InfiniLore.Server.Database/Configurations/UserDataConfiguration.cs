// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Server.Database.Configurations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UserDataConfiguration<TModel> : IEntityTypeConfiguration<TModel> where TModel : UserData {
    public virtual void Configure(EntityTypeBuilder<TModel> builder) {
        builder.HasOne(x => x.Owner)
            .WithMany()
            .HasForeignKey(x => x.OwnerId)
            .IsRequired();

        builder.UseTptMappingStrategy();
    }
}
