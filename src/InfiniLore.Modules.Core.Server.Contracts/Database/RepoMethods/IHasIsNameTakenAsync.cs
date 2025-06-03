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
    ValueTask<Shared.Outcome> IsNameTakenAsync(string name, Guid ownerId, Guid notIncludedId = default, CancellationToken ct = default);
    ValueTask<Shared.Outcome> IsNameNotTakenAsync(string name, Guid ownerId, Guid notIncludedId = default, CancellationToken ct = default);
}
