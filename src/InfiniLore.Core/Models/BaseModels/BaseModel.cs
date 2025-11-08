// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Core.Models.BaseModels;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BaseModel {
    public Guid Id { get; } = Guid.CreateVersion7();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public bool IsDeleted => DeletedAt != DateTime.MinValue;
    public DateTime DeletedAt { get; set; } = DateTime.MinValue;

    public bool IsArchived => ArchivedAt != DateTime.MinValue;
    public DateTime ArchivedAt { get; set; } = DateTime.MinValue;
}

public class BaseModelConfiguration : IEntityTypeConfiguration<BaseModel> {
    public void Configure(EntityTypeBuilder<BaseModel> builder) {
        builder.HasKey(a => a.Id);
        
        builder.Ignore(a => a.IsDeleted);
        builder.Ignore(a => a.IsArchived);
    }
}