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
    public InfiniLoreUserModel? ModelOwner { get; set; } = null;
    
    public ICollection<AccessProtectionRuleModel>? Rules { get; set; } = null;

    [NotMapped] public FrozenDictionary<Guid, AccessProtectionRuleModel[]> UserMappedRules => Rules?
            .GroupBy(rule => rule.UserId)
            .ToFrozenDictionary(group => group.Key, group => group.ToArray())
        ?? FrozenDictionary<Guid, AccessProtectionRuleModel[]>.Empty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool HasPermission(Guid userId, string permission) {
        if (userId == ModelOwnerId) return true;
        return UserMappedRules.TryGetValue(userId, out AccessProtectionRuleModel[]? rules)
            && rules.Any(rule => rule.Permission == permission);
    }

    public void GrantPermission(Guid userId, string permission) {
        if (userId == ModelOwnerId) return;
        if (!UserMappedRules.TryGetValue(userId, out AccessProtectionRuleModel[]? rules)) {
            Rules ??= new List<AccessProtectionRuleModel>();
            Rules.Add(new AccessProtectionRuleModel {
                UserId = userId,
                Permission = permission
            });
            return;
        }
        
        // If the permission is already granted, return.
        if (rules.FirstOrDefault(r => r.Permission == permission) is not null) return;
        
        Rules ??= new List<AccessProtectionRuleModel>();
        Rules.Add(new AccessProtectionRuleModel {
            UserId = userId,
            Permission = permission
        });
    }

    public void RevokePermission(Guid userId, string permission) {
        if (userId == ModelOwnerId) return;
        if (!UserMappedRules.TryGetValue(userId, out AccessProtectionRuleModel[]? rules)) return;
        if (rules.FirstOrDefault(r => r.Permission == permission) is not {} foundRule) return;
        Rules?.Remove(foundRule);
    }
}