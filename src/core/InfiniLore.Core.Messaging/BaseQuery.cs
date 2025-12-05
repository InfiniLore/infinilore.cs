// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Core.Database;
using InfiniLore.Core.Outcomes;

namespace InfiniLore.Core.Messaging;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract record BaseQuery<TResult> : ICommand<Outcome<TResult>> {
    public QueryConfig Config { get; init; } = QueryConfig.None;
}