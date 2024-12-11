// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using MediatR;
using Serilog;

namespace InfiniLore.Server.Services.CQRS.PipelineBehaviours;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ReturnLoggingBehavior<TRequest, TResponse>(ILogger logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : ITryGetAsFailureValue<string> {

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
        // Code that runs before the Handler is called
        TResponse response = await next();

        // Code that runs after the Handler is called
        if (response.TryGetAsFailureValue(out string? output)) {
            logger.Error("Request failed: {Request} - {Output}", request, output);
        }

        return response;
    }
}
