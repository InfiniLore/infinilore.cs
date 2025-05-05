// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Database.Models;
using System.Diagnostics.CodeAnalysis;

namespace InfiniLore.Server.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class OwnedData<TOwner> : BasicData, IOwnedData<TOwner>
    where TOwner : class, IBasicData 
{
    public Guid OwnerId { get; set; } = Guid.Empty;
    public TOwner? Owner { get; set; } = default;
    [MemberNotNullWhen(true, nameof(Owner))] public bool IsOwnerIncluded => Owner != null; 
}
