// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Contracts.Services.Mediator;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.Mediator.PipelineBehaviours;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ChallengeAccessDataBehaviour<TRequest>(
    ILogger<ChallengeAccessDataBehaviour<TRequest>> logger
)
    : IPreProcessor<TRequest>
    where TRequest : IMediatorRequest {
    public Task PreProcessAsync(IPreProcessorContext<TRequest> ctx, CancellationToken ct) {
        logger.LogWarning("Challenge access data not implemented");
        return Task.CompletedTask;
    }
}
