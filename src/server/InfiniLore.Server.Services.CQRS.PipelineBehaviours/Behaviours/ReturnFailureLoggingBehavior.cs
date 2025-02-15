// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using MediatR;
using Serilog;

namespace InfiniLore.Server.Services.CQRS.PipelineBehaviours.Behaviours;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ReturnFailureLoggingBehavior<TRequest, TResponse>(ILogger logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ITryGetAsFailureValue<string> {

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
        // Run before next step

        // Execute next step
        TResponse response = await next();

        // run after next step
        if (response.TryGetAsFailureValue(out string? output)) {
            logger.Error("Request failed: {Request} - \"{Output}\"", request, output);
        }

        // exit
        return response;
    }
}
