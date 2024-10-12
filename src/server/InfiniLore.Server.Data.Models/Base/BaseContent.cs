// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfiniLore.Server.Data.Models.Base;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BaseContent<T> where T : BaseContent<T> {
    [Key] public Guid Id { get; set; }

    #region Tracking
    public DateTime CreatedDate { get; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

    // ReSharper disable once CollectionNeverUpdated.Global
    public ICollection<AuditLog<T>> AuditLogs { get; set; } = [];
    #endregion

    #region SoftDelete
    [NotMapped] public bool IsSoftDeleted => SoftDeleteDate != null;
    public DateTime? SoftDeleteDate { get; private set; }
    public void SoftDelete() => SoftDeleteDate = DateTime.UtcNow;
    #endregion
}
