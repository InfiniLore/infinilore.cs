// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Tests.Core.Modular.Data;

namespace Tests.Core.Modular;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniModuleTests {

    [Test]
    public async Task Load_ShouldRegisterServicesFromModule() {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var module = new TestInfiniModule2();

        // Act
        module.Load(serviceCollection);

        // Assert
        await Assert.That(serviceCollection).Contains(descriptor => descriptor.ServiceType == typeof(SomeService2));
    }

    [Test]
    public async Task Load_ShouldRegisterNestedServicesFromSubmodules() {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var module = new TestInfiniModule1();

        // Act
        module.Load(serviceCollection);

        // Assert
        await Assert.That(serviceCollection)
            .Contains(descriptor => descriptor.ServiceType == typeof(SomeService1))
            .Contains(descriptor => descriptor.ServiceType == typeof(SomeService2));
    }
    
    [Test]
    public async Task Load_ShouldRegisterAssemblyFromModule() {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var module = new TestInfiniModule2();

        // Act
        module.Load(serviceCollection);

        // Assert
        await Assert.That(module.Assemblies)
            .Count(assembly => assembly.Equals(typeof(TestInfiniModule1).Assembly)).IsEqualTo(1);
    }
    
    
    [Test]
    public async Task Load_ShouldRegisterAssemblyFromModule_nestedModules() {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var module = new TestInfiniModule1();

        // Act
        module.Load(serviceCollection);

        // Assert
        await Assert.That(module.Assemblies)
            .HasCount(1)
            .Count(assembly => assembly.Equals(typeof(TestInfiniModule1).Assembly)).IsEqualTo(1).Because("Should contain assembly from modules and submodules distinctly");
    }
}
