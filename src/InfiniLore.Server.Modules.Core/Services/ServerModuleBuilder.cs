// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Builder;
using Serilog;
using System.Reflection;

namespace InfiniLore.Server.Modules.Core;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ServerModuleBuilder {
    private WebApplicationBuilder AppBuilder { get; init; } = null!;
    private ServerModuleBuilder() {}
    public List<Assembly> ModuleAssemblies { get; } = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static ServerModuleBuilder CreateFromBuilder(WebApplicationBuilder builder) {
        return new ServerModuleBuilder {
            AppBuilder = builder
        };
    }

    public ServerModuleBuilder AddModule<TAssemblyEntry>() {
        Assembly assembly = typeof(TAssemblyEntry).Assembly;
        
        TypeInfo? serverModuleSetupType = assembly.DefinedTypes.FirstOrDefault(t => t.IsAssignableTo(typeof(IServerModuleSetup)));
        if (serverModuleSetupType is null) {
            Log.Logger.Error("Could not find a server module setup type in assembly {AssemblyName}.", assembly.GetName().Name);
            return this;
        }
        
        Log.Logger.Information("Found server module setup type {ServerModuleSetupType} in assembly {AssemblyName}.", serverModuleSetupType.Name, assembly.GetName().Name);
        var serverModuleSetup = (IServerModuleSetup)Activator.CreateInstance(serverModuleSetupType.AsType())!;
        serverModuleSetup.Setup(AppBuilder); 
        
        ModuleAssemblies.Add(assembly);
        
        return this;
    }
}
