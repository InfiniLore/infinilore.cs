// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Server.Data.Models.Base;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AuditLog<T> where T : BaseContent<T> {
    [Key] public Guid Id { get; set; }
    
    public T Content { get; set; }
    public Guid ContentId { get; set; }
    public string UserId { get; set; }
    
    public DateTime ChangeDate { get; set; }
    public string ChangeType { get; set; } // TODO add ChangeType predefined stuff : "Created", "Updated", "Deleted"
    public string ChangedProperties { get; set; } // TODO make a better thing of this JSON of changed properties
}
