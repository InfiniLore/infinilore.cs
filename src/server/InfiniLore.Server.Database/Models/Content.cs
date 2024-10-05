// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfiniLore.Server.Database.Models;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class Content : ISoftDeletable {
    [Key] public Guid Id { get; set; }
    
    #region SoftDelete
    [NotMapped] public bool IsSoftDeleted => SoftDeleteDate != null;
    public DateTime? SoftDeleteDate { get; private set; }
    public void SoftDelete() => SoftDeleteDate = DateTime.UtcNow;
    #endregion
}