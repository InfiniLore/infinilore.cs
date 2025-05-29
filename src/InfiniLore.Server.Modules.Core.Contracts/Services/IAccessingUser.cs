// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Frozen;
using System.Collections.Immutable;

namespace InfiniLore.Server.Modules.Core;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAccessingUser {
    Guid UserId { get; }
    ImmutableArray<string> Roles { get; }
    ImmutableArray<string> Permissions { get; }
    FrozenDictionary<string, object> MetaData { get; }
}
