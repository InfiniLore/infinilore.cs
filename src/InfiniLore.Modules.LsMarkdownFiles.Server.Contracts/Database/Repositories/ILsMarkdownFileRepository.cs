// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Database.RepoMethods;
using InfiniLore.Server.Modules.LoreScopes.Database;

namespace InfiniLore.Server.Modules.LsMarkdownFiles.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILsMarkdownFileRepository : IOwnedModelRepository<LoreScopeModel, LsMarkdownFileModel>,
    IHasIsNameTakenAsync<LsMarkdownFileModel>;
