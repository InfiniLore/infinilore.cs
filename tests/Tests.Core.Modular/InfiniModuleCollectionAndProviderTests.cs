// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Modular;
using Microsoft.Extensions.DependencyInjection;
using Tests.Core.Modular.Data;

namespace Tests.Core.Modular;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniModuleCollectionAndProviderTests {
    [Test]
    public async Task Build_ShouldLoadAllModulesAndRegisterTheirServices() {
        // Arrange
        var serviceCollection = new ServiceCollection();
        InfiniModuleCollection collection = InfiniModuleCollection.Create()
            .AddModule<TestInfiniModule1>()
            .AddModule<TestInfiniModule2>();

        // Act
        InfiniModuleProvider _ = collection.Build(serviceCollection);

        // Assert
        await Assert.That(serviceCollection)
            .Contains(descriptor => descriptor.ServiceType == typeof(SomeService1))
            .Contains(descriptor => descriptor.ServiceType == typeof(SomeService2));
    }

    [Test]
    public async Task Build_ShouldReturnProviderWithAllModules() {
        // Arrange
        var services = new ServiceCollection();
        InfiniModuleCollection collection = InfiniModuleCollection.Create()
            .AddModule<TestInfiniModule1>()
            .AddModule<TestInfiniModule2>();

        // Act
        InfiniModuleProvider moduleProvider = collection.Build(services);

        // Assert
        await Assert.That(moduleProvider.Modules)
            .HasCount(2)
            .Contains(module => module is TestInfiniModule1)
            .Contains(module => module is TestInfiniModule2);
    }

    [Test]
    public async Task Build_ShouldReturnProviderWithDistinctAssembliesFromAllModules() {
        // Arrange
        var services = new ServiceCollection();
        InfiniModuleCollection collection = InfiniModuleCollection.Create()
            .AddModule<TestInfiniModule1>()
            .AddModule<TestInfiniModule2>();

        // Act
        InfiniModuleProvider moduleProvider = collection.Build(services);

        // Assert
        await Assert.That(moduleProvider.Assemblies)
            .HasCount(1)
            .Count(assembly => assembly.Equals(typeof(TestInfiniModule1).Assembly)).IsEqualTo(1).Because("Assemblies from all modules should be aggregated and distinct.");
    }

    [Test]
    public async Task Create_ShouldReturnEmptyCollectionThatBuildsToEmptyProvider() {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var collection = InfiniModuleCollection.Create();
        InfiniModuleProvider moduleProvider = collection.Build(services);

        // Assert
        await Assert.That(moduleProvider.Modules)
            .HasCount(0);

        await Assert.That(moduleProvider.Assemblies)
            .HasCount(0);
    }
}
