// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Server.Modules.Core.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AccessProtectionRuleModel : OwnedModel<AccessProtectionModel> {
    public Guid UserId { get; init; }
    [MaxLength(Defaults.PermissionMaxLength)] public string Permission { get; set; } = null!;
    
    public static class Defaults {
        public const int PermissionMaxLength = 256;
    }
}
