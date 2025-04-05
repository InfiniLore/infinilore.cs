// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Services.Mediator;
using Newtonsoft.Json;

namespace InfiniLore.Server.Services.Mediator;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record CommonRequestData : ICommonRequestData {
    [JsonProperty("access_data")] public IAccessData AccessData { get; init; } = RequestAccessData.Empty;
    [JsonProperty("created_at")] public DateTime CreatedAt { get; } = DateTime.UtcNow;
}

public record MediatorRequest : CommonRequestData;

// ReSharper disable once UnusedTypeParameter
public record MediatorRequest<TResponse> : CommonRequestData;
