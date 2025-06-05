// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared.Database;

namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once UnusedTypeParameter
public interface IHasIsNameTakenAsync<TModel> where TModel : BasicModel, IHasName, IHasOwnerId {
    ValueTask<RepoOutcome> IsNameTakenAsync(string name, Guid ownerId, Guid notIncludedId = default, CancellationToken ct = default);
    ValueTask<RepoOutcome> IsNameNotTakenAsync(string name, Guid ownerId, Guid notIncludedId = default, CancellationToken ct = default);
}
