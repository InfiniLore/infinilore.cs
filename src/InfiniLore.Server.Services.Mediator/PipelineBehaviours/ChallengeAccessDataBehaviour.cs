// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Services.Mediator;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.Mediator.PipelineBehaviours;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ChallengeAccessDataBehaviour<TRequest, TResponse>(
    ILogger<ChallengeAccessDataBehaviour<TRequest, TResponse>> logger
)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMediatorRequest {
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct) {
        logger.LogWarning("Challenge access data not implemented");
        return await next();
    }
}
