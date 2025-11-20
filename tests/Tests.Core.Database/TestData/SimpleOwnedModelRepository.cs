// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Core.Database;

namespace Tests.Core.Database.TestData;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<SimpleOwnedModelRepository>]
public class SimpleOwnedModelRepository : BaseOwnerRepository<SimpleOwnedModel, SimpleOwnerModel>;
