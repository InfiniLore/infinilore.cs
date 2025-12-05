// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Modular;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Core.Modular.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TestInfiniModule1 : InfiniModule {

    protected override void OnConfiguring(IServiceCollection services) {
        services.AddSingleton<SomeService1>();

        AddSubModule<TestInfiniModule2>();
    }
    
    protected override void OnStartup(WebApplication app) {
        
    }
}

public class TestInfiniModule2 : InfiniModule {

    protected override void OnConfiguring(IServiceCollection services) {
        services.AddSingleton<SomeService2>();
    }
    
    protected override void OnStartup(WebApplication app) {
        
    }
}

public class SomeService1;

public class SomeService2;
