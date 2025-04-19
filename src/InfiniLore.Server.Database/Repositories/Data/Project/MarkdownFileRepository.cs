// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Contracts.Database.Repositories.Data.Project;
using InfiniLore.Server.Database.Models.Data.Project;

namespace InfiniLore.Server.Database.Repositories.Data.Project;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IMarkdownFileRepository>()]
public class MarkdownFileRepository : ProjectDataRepository<MarkdownFile>, IMarkdownFileRepository {
    
}
