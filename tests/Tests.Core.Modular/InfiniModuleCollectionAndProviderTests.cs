// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Modular;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
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
        InfiniModuleCollection collection = new InfiniModuleCollection(serviceCollection)
            .AddModule<TestInfiniModule1>()
            .AddModule<TestInfiniModule2>();

        // Act
        InfiniModuleProvider _ = collection.Build();

        // Assert
        await Assert.That(serviceCollection)
            .Contains(descriptor => descriptor.ServiceType == typeof(SomeService1))
            .Contains(descriptor => descriptor.ServiceType == typeof(SomeService2));
    }

    [Test]
    public async Task Build_ShouldReturnProviderWithAllModules() {
        // Arrange
        var services = new ServiceCollection();
        InfiniModuleCollection collection = new InfiniModuleCollection(services)
            .AddModule<TestInfiniModule1>()
            .AddModule<TestInfiniModule2>();

        // Act
        InfiniModuleProvider moduleProvider = collection.Build();

        // Assert
        await Assert.That(moduleProvider.Modules)
                .Contains(module => module.UnderlyingModule is TestInfiniModule1)
                .Contains(module => module.UnderlyingModule is TestInfiniModule2)
                .Count().IsEqualTo(2)
            ;
    }

    [Test]
    public async Task Build_ShouldReturnProviderWithDistinctAssembliesFromAllModules() {
        // Arrange
        var services = new ServiceCollection();
        InfiniModuleCollection collection = new InfiniModuleCollection(services)
            .AddModule<TestInfiniModule1>()
            .AddModule<TestInfiniModule2>();

        // Act
        InfiniModuleProvider moduleProvider = collection.Build();

        // Assert
        Assembly[] assemblies = moduleProvider.GetRegisteredAssemblies().ToArray();
        
        await Assert.That(assemblies).Count().IsEqualTo(1);
        await Assert.That(assemblies).Count(assembly => assembly.Equals(typeof(TestInfiniModule1).Assembly)).IsEqualTo(1).Because("Assemblies from all modules should be aggregated and distinct.");
    }

    [Test]
    public async Task Create_ShouldReturnEmptyCollectionThatBuildsToEmptyProvider() {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var collection = new InfiniModuleCollection(services);
        InfiniModuleProvider moduleProvider = collection.Build();

        // Assert
        await Assert.That(moduleProvider.Modules)
            .Count().IsEqualTo(0);
        
        
        Assembly[] assemblies = moduleProvider.GetRegisteredAssemblies().ToArray();

        await Assert.That(assemblies)
            .Count().IsEqualTo(0);
    }
}
