// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Newtonsoft.Json;

namespace InfiniLore.Server.Modules.Core.Messaging;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record CommonRequestData : ICommonRequestData {
    [JsonProperty("access_data")] public IAccessData AccessData { get; init; } = RequestAccessData.Empty;
    [JsonProperty("created_at")] public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
