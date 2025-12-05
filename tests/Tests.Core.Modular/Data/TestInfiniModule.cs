// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Modular;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Core.Modular.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TestInfiniModule1 : InfiniModule {

    protected override void Configure() {
        Services.AddSingleton<SomeService1>();

        AddSubModule<TestInfiniModule2>();
    }
}

public class TestInfiniModule2 : InfiniModule {

    protected override void Configure() {
        Services.AddSingleton<SomeService2>();
    }
}

public class SomeService1;

public class SomeService2;
