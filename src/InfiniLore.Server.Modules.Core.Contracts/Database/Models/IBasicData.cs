// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Server.Modules.Core.Database.Models;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IBasicData {
    public Guid Id { get; init; }
    public DateTime CreatedDate { get; }
    public DateTime LastModifiedDate { get;  }
    public void UpdateLastModifiedDate();

    public bool IsSoftDeleted { get; }
    public DateTime? SoftDeleteDate { get; }
    public void SoftDelete();
}
