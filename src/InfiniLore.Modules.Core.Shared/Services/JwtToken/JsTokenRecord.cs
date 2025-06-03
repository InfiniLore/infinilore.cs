// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;

namespace InfiniLore.Modules.Core.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly] public class JsTokenRecord {
    [UsedImplicitly] public string Id { get; set; } = null!;
    [UsedImplicitly] public string? Value { get; set; }
    [UsedImplicitly] public string? ExpiresAt { get; set; }// ISO 8601 formatted expiration timestamp
}
