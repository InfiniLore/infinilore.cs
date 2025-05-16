// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Shared.Modules.Core.Database;

namespace InfiniLore.Server.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class OwnedModel<TOwner> : BasicModel, IOwnedModel
    where TOwner : BasicModel 
{
    public Guid OwnerId { get; set; } = Guid.Empty;
    
    private TOwner? _owner;
    public TOwner? Owner {
        get => _owner;
        set {
            _owner = value;
            OwnerId = value?.Id ?? Guid.Empty;
        }
    }
}
