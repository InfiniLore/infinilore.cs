// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace InfiniLore.Server.Modules.Core.Database.Models;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IOwnedData<TOwner> : IBasicData {
    Guid OwnerId { get; set; }
    TOwner? Owner { get; set; }
    [MemberNotNullWhen(true, nameof(Owner))] bool IsOwnerIncluded { get; }
}
