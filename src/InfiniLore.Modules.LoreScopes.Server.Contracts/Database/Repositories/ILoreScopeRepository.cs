// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Database.RepoMethods;

namespace InfiniLore.Server.Modules.LoreScopes.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILoreScopeRepository : IOwnedModelRepository<InfiniLoreUserModel, LoreScopeModel>,
    IHasAccessPermissionAsync,
    IHasIsNameTakenAsync<LoreScopeModel>;
