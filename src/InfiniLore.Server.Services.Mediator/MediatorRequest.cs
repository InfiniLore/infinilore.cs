// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Services.Cqrs;
using MediatR;

namespace InfiniLore.Server.Services.Mediator;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record CommonRequestData : IMediatorRequest {
    public IAccessData AccessData { get; init; } = RequestAccessData.Empty;

    public DateTime CreatedAt { get; } = DateTime.UtcNow;
}

public record MediatorRequest : CommonRequestData, IRequest<MediatorResponse>;

public record MediatorRequest<TResponse> : CommonRequestData, IRequest<MediatorResponse<TResponse>>;
