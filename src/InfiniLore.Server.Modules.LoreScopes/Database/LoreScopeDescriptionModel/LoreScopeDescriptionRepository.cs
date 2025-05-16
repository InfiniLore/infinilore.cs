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
[InjectableService<ILoreScopeDescriptionRepository>(ServiceLifetime.Scoped)]
public class LoreScopeDescriptionRepository : MarkdownDocumentRepository<LoreScopeModel, LoreScopeDescriptionModel>, ILoreScopeDescriptionRepository;
