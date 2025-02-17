// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore.Query;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfiniLore.Server.Database.Models;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class BasicData {
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;
    public DateTime LastModifiedDate { get; private set; } = DateTime.UtcNow;

    #region UpdateLastModifiedDate
    public void UpdateLastModifiedDate() => LastModifiedDate = DateTime.UtcNow;
    public static SetPropertyCalls<T> UpdateLastModifiedDate<T>(SetPropertyCalls<T> setPropertyCalls) where T : BasicData =>
        setPropertyCalls.SetProperty(propertyExpression: x => x.LastModifiedDate, DateTime.UtcNow);
    #endregion

    #region SoftDelete
    [NotMapped] public bool IsSoftDeleted => SoftDeleteDate != null;
    public DateTime? SoftDeleteDate { get; private set; }
    public void SoftDelete() {
        SoftDeleteDate = DateTime.UtcNow;
        UpdateLastModifiedDate();
    }

    public static SetPropertyCalls<T> SoftDelete<T>(SetPropertyCalls<T> setPropertyCalls) where T : BasicData =>
        setPropertyCalls
            .SetProperty(propertyExpression: x => x.SoftDeleteDate, DateTime.UtcNow)
            .SetProperty(propertyExpression: x => x.LastModifiedDate, DateTime.UtcNow);
    #endregion
}
