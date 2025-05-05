// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Shared.Modules.Core.Database;
using System.Diagnostics.CodeAnalysis;

namespace InfiniLore.Server.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class OwnedModel<TOwner> : BasicModel, IOwnedModel
    where TOwner : BasicModel 
{
    public Guid OwnerId { get; set; } = Guid.Empty;
    public TOwner? Owner { get; set; }
    [MemberNotNullWhen(true, nameof(Owner))] public bool IsOwnerIncluded => Owner != null; 
}
