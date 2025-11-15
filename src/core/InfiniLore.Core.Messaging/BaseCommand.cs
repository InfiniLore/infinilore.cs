// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Core.Outcomes;

namespace InfiniLore.Core.Messaging;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract record BaseCommand<TResult> : BaseMessage, ICommand<Outcome<TResult>>;
public abstract record BaseCommand<TSuccess, TFailure> : BaseMessage, ICommand<Outcome<TSuccess, TFailure>>;
