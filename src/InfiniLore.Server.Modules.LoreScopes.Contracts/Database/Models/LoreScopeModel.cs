// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Database;
using InfiniLore.Server.Modules.Users.Database;
using InfiniLore.Shared.Modules.LoreScopes.Database;
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Server.Modules.LoreScopes.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoreScopeModel : OwnedModel<InfiniLoreUserModel>, ILoreScopeModel {
    [MaxLength(Defaults.NameMaxLength)] public string Name { get; set; } = "";

    private LoreScopeDescriptionModel? _description;
    public LoreScopeDescriptionModel? Description {
        get => _description;
        set  {
            _description = value;
            DescriptionId = value?.Id;
        }
    }
    public Guid? DescriptionId { get; set; }
    
    public bool HasDescription => DescriptionId is not null && DescriptionId != Guid.Empty;
    
    public static class Defaults {
        public const int NameMaxLength = 100;
    }
}
