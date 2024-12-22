// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Services.CQRS.PipelineBehaviours.Behaviours;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Services.CQRS.PipelineBehaviours;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ManualAssignmentExtensions {
    public static MediatRServiceConfiguration AddInfinilorePipelineBehaviours(this MediatRServiceConfiguration configuration) {

        // see https://github.com/jbogard/MediatR/wiki/Behaviors#registering-pipeline-behaviors
        configuration.AddOpenBehavior(typeof(StoreCommandsBehaviour<,>));
        configuration.AddOpenBehavior(typeof(ReturnLoggingBehavior<,>));
        
        return configuration;
    }
}
