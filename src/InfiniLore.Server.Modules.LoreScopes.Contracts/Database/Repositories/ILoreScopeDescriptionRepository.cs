// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Database;

namespace InfiniLore.Server.Modules.LoreScopes.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILoreScopeDescriptionRepository : IMarkdownDocumentRepository<LoreScopeModel, LoreScopeDescriptionModel>;
