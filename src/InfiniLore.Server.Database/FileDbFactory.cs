// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Minio;
using Serilog;
using Testcontainers.Minio;
using ILogger=Microsoft.Extensions.Logging.ILogger;

namespace InfiniLore.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class FileDbFactory {
    private static readonly ILoggerFactory EmptyLoggerFactory = LoggerFactory.Create(builder => builder.AddSerilog(Log.Logger));
    public const string AccessKey = "minioadmin"; // TODO make MinIo AccessKey into config settings
    public const string SecretKey = "minioadmin"; // TODO make MinIo SecretKey into config settings
    public const int Port = 40627; // TODO make MinIo Port into config settings
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static async Task CreateDockerMinIoContainer() {
        ILogger logger = EmptyLoggerFactory.CreateLogger("DOCKER mssql");

        MinioContainer container = new MinioBuilder()
            .WithPortBinding(Port, MinioBuilder.MinioPort)
            .WithLogger(logger)
            .WithImage("minio/minio")
            .WithName("infinilore-development-db-file")
            .WithReuse(true)
            .WithLabel("reuse-id", "infinilore-development-db-file")
            .WithEnvironment("MINIO_ROOT_USER", AccessKey)
            .WithEnvironment("MINIO_ROOT_PASSWORD", SecretKey)
            .Build();

        try {
            await container.StartAsync();
            logger.LogInformation("Database connection string: {ConnectionString}", container.GetConnectionString());
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to start MinIO container. Attempting cleanup and retry...");
            try {
                await container.DisposeAsync();
            }
            catch (Exception e) {
                logger.LogError(e, "Error during cleanup");
            }
            throw;
        }

    }

    public static void RegisterDatabase(
        IServiceCollection services,
        Action<IMinioClient> options
    ) {
        services.AddMinio(options);
        services.AddSingleton<MinIoFileDb>();
    }
}
