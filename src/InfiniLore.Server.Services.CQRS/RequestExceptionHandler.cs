// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.CQRS;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RequestExceptionHandler<TRequest, TException, T>(
    ILoggerFactory loggerFactory
) : IRequestExceptionHandler<TRequest, MediatorResponse<T>, TException> 
    where TRequest : notnull 
    where TException : Exception
{
    
    private readonly ILogger logger = loggerFactory.CreateLogger("EXCEPTION");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<MediatorResponse<T>> state, CancellationToken cancellationToken) {
        logger.Warning($"Exception caught in request handler for request type {typeof(TRequest).Name}: {exception.Message}");
        
        // TODO Store exception to some DB
        #if RELEASE
        state.SetHandled(MediatorResponse<T>.FromFailureString(exception.Message));
        #endif
        return Task.CompletedTask;
    }
}
