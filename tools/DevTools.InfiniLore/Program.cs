// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;
using CodeOfChaos.CliArgsParser.Library;
using DevTools.InfiniLore.Library;
using InfiniLore.Credentials.Auth0.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace DevTools.InfiniLore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public static async Task Main(string[] args) {
        IServiceProvider provider = InitializeServiceProvider();

        // Register & Build the parser
        //      Don't forget to add the current assembly if you built more tools for the current project
        ICliParser parser = CliParser.CreateBuilder()
            .AddFromAssembly<IAssemblyEntry>()
            .AddFromAssembly(typeof(Program).Assembly)
            .WithServiceProvider(provider)
            .Build();

        // We are doing this here because else the launchSettings.json file becomes a humongous issue to deal with.
        //      Sometimes CLI params are not the answer.    
        //      Code is the true savior
        string projects = string.Join(";",
            "Old.InfiniLore.Server.Types",
            "Old.InfiniLore.Contracts"
        );

        string oneLineArgs = ArgsInputHelper.ToOneLine(args).Replace("%PROJECTS%", projects);
        await parser.ExecuteAsync(oneLineArgs);
    }
    
    private static IServiceProvider InitializeServiceProvider() {
        #region Configuration
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddUserSecrets<IDevToolsEntry>()
            .Build();
        #endregion
        
        var services = new ServiceCollection();

        #region Logging
        LoggerConfiguration loggerConfig = new LoggerConfiguration()
            .AsAnnaSasDevServerConsole(
                24,
                configure: asyncConsoleConfig => asyncConsoleConfig.ApplyThemeToRedirectedOutput = true// Needed for nice DotnetWatch console output    
            )
            .WithTruncateSourceContextEnricher(maxLength: 24)
            .MinimumLevel.Debug();

        Log.Logger = loggerConfig.CreateLogger();

        // Clear default providers and setup Serilog
        services.AddLogging(loggingBuilder => {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(Log.Logger, dispose: true);
        });
        services.AddSingleton(Log.Logger);
        #endregion
        
        #region Auth0
        services.AddAuth0ManagementApiServices(configuration, configure: config => {
            config.Auth0Options.Domain = configuration["Auth0:Domain"] ?? string.Empty;
            config.Auth0Options.ClientId = configuration["Auth0:ClientId-Management"] ?? string.Empty;
            config.Auth0Options.ClientSecret = configuration["Auth0:ClientSecret-Management"] ?? string.Empty;

            config.SetScopedTokenStore<NetUserSecretAuth0AccessTokenStore>();
            config.Auth0Options.AccessToken = configuration["Auth0:AccessToken"];
        });
        #endregion

        services.RegisterServicesFromDevToolsInfiniLore();
        return services.BuildServiceProvider();
    }
}
