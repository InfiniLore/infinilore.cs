// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace InfiniLore.Server.Data.Models.Base;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[SuppressMessage("ReSharper", "EntityFramework.ModelValidation.UnlimitedStringLength")]
public class AuditLog<T> where T : BaseContent<T> {
    [Key] public Guid Id { get; set; }
    
    public required T Content { get; set; }
    public required Guid ContentId { get; set; }
    public required Guid UserId { get; set; }
    
    public DateTime ChangeDate { get; set; }
    public string ChangeType { get; set; } = string.Empty; // TODO add ChangeType predefined stuff : "Created", "Updated", "Deleted"
    public string ChangedProperties { get; set; } = string.Empty; // TODO make a better thing of this JSON of changed properties
}
