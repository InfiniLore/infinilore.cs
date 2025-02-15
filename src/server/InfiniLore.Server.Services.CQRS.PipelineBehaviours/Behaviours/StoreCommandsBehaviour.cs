// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Services.CQRS;
using MediatR;

namespace InfiniLore.Server.Services.CQRS.PipelineBehaviours.Behaviours;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class StoreCommandsBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ICqrsCommand
    where TResponse : notnull{

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
        // Run before next step
        // TODO use a Document-Oriented db like MongoDb to store the command
        //      We shouldn't wait for the result necessarily, as we can  just fire and forget.

        // Execute next step
        TResponse response = await next();

        // run after next step

        // exit
        return response;
    }
    
}
