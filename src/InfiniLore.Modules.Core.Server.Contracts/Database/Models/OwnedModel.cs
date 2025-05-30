// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared.Database;

namespace InfiniLore.Modules.Core.Server.Database;
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
