// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;

namespace InfiniLore.Server.Modules.Core;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ServerModuleBuilder {
    private IServiceCollection? Services { get; init; }
    private WebApplicationBuilder? AppBuilder { get; init; }
    private ServerModuleBuilder() {}
    public List<Assembly> ModuleAssemblies { get; } = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static ServerModuleBuilder Create(WebApplicationBuilder builder) {
        return new ServerModuleBuilder {
            AppBuilder = builder,
            Services = builder.Services
        };
    }

    public static ServerModuleBuilder Create(IServiceCollection services) {
        return new ServerModuleBuilder {
            AppBuilder = null,
            Services = services
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
        
        if (AppBuilder is not null) serverModuleSetup.SetupBuilder(AppBuilder);
        if (Services is not null) serverModuleSetup.SetupServices(Services);
        
        ModuleAssemblies.Add(assembly);
        
        return this;
    }
}
