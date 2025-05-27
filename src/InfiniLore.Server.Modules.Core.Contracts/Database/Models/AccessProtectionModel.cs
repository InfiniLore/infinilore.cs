// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Frozen;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfiniLore.Server.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AccessProtectionModel : BasicModel {
    public Guid ProtectedModelId { get; init; } = Guid.Empty;
    public Guid ModelOwnerId { get; init; } = Guid.Empty;
    public InfiniLoreUserModel? ModelOwner { get; set; }

    public ICollection<AccessProtectionRuleModel>? Rules { get; set; }

    [NotMapped] public FrozenDictionary<Guid, AccessProtectionRuleModel[]> UserMappedRules => Rules?
            .GroupBy(rule => rule.UserId)
            .ToFrozenDictionary(group => group.Key, group => group.ToArray())
        ?? FrozenDictionary<Guid, AccessProtectionRuleModel[]>.Empty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool HasPermission(Guid userId, string? permission = null) {
        if (userId == ModelOwnerId) return true;
        if (permission.IsNullOrWhiteSpace()) return false;

        return UserMappedRules.TryGetValue(userId, out AccessProtectionRuleModel[]? rules)
            && rules.Any(rule => rule.Permission == permission);
    }

    public bool TryGrantPermission(Guid userId, string permission) {
        if (userId == ModelOwnerId) return true;
        if (permission.IsNullOrWhiteSpace()) return false;
        if (permission.Length > AccessProtectionRuleModel.Defaults.PermissionMaxLength) return false;

        if (!UserMappedRules.TryGetValue(userId, out AccessProtectionRuleModel[]? rules)) {
            Rules ??= new List<AccessProtectionRuleModel>();
            Rules.Add(new AccessProtectionRuleModel {
                UserId = userId,
                Permission = permission
            });
            return true;
        }

        // If the permission is already granted, return.
        if (rules.FirstOrDefault(r => r.Permission == permission) is not null) return false;

        Rules ??= new List<AccessProtectionRuleModel>();
        Rules.Add(new AccessProtectionRuleModel {
            UserId = userId,
            Permission = permission
        });
        return true;
    }

    public bool TryRevokePermission(Guid userId, string permission) {
        if (userId == ModelOwnerId) return false;
        if (!UserMappedRules.TryGetValue(userId, out AccessProtectionRuleModel[]? rules)) return false;
        if (rules.FirstOrDefault(r => r.Permission == permission) is not {} foundRule) return false ;

        return Rules?.Remove(foundRule) ?? false;
    }
}
