// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Modules.Core.Database;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Modules.LoreScopes.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<ILoreScopeDocumentRepository>(ServiceLifetime.Scoped)]
public class LoreScopeDocumentRepository : OwnedModelRepository<LoreScopeModel, LoreScopeDocumentModel>, ILoreScopeDocumentRepository;
