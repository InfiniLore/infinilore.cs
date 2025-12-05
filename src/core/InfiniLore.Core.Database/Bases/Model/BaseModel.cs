// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract record BaseModel {
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

    public bool IsSoftDeleted => SoftDeletedAt != DateTime.MinValue;
    public DateTime SoftDeletedAt { get; set; } = DateTime.MinValue;

    [Timestamp] public byte[]? RowVersion { get; set; } = null;
}