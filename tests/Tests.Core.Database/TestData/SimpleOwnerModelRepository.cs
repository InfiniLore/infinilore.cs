// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Core.Database;

namespace Tests.Core.Database.TestData;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<SimpleOwnerModelRepository>]
public class SimpleOwnerModelRepository : BaseModelRepository<SimpleModel>;
