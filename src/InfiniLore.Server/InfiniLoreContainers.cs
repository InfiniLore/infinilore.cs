// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Serilog;
using Testcontainers.Minio;
using Testcontainers.MsSql;
using ILogger=Microsoft.Extensions.Logging.ILogger;

namespace InfiniLore.Server;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniLoreContainers : IAsyncDisposable {
    private MsSqlContainer SqlContainer { get; init; } = null!;
    private MinioContainer MinioContainer { get; init; } = null!;

    private static readonly ILogger Logger = LoggerFactory
        .Create(builder => builder.AddSerilog(Log.Logger))
        .CreateLogger("DOCKER-DEV-ENV");
    
    private const int SqlPort = 40626;
    private const string SqlPassword = "AnnaIsTrans4Ever!";

    private const int MinioPort = 40627;
    private const int MinioWebUiPort = 40628;
    
    private const string MinioAccessKey = "minioadmin";
    private const string MinioSecretKey = "minioadmin";

    private const string ProjectLabel = "infinilore-dev";
    private const string SqlImage = "mcr.microsoft.com/mssql/server:2022-latest";
    private const string MinioImage = "minio/minio:latest";

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    private InfiniLoreContainers() {}

    public static InfiniLoreContainers CreateForDevelopment() {
        // Configure an SQL Server container
        MsSqlBuilder sqlBuilder = new MsSqlBuilder()
            .WithLogger(Logger)
            .WithImage(SqlImage)
            .WithPortBinding(SqlPort, MsSqlBuilder.MsSqlPort)
            .WithPassword(SqlPassword)
            .WithName("infinilore-dev-content")
            .WithReuse(true)
            .WithLabel("reuse-id", "infinilore-dev-content")
            .WithLabel("service", "mssql")
            .WithLabel("com.docker.compose.project", ProjectLabel)
            .WithLabel("com.docker.compose.service", "mssql")
            .WithLabel("com.docker.compose.version", "1.0")
            .WithLabel("com.docker.compose.container-number", "1")
            .WithLabel("com.docker.compose.depends_on", "");

        // Configure a MinIO container
        MinioBuilder? minIoBuilder = new MinioBuilder()
            .WithLogger(Logger)
            .WithImage(MinioImage)
            .WithPortBinding(MinioPort, MinioBuilder.MinioPort)
            .WithPortBinding(MinioWebUiPort, 40585)
            .WithUsername(MinioAccessKey)
            .WithPassword(MinioSecretKey)
            .WithName("infinilore-dev-s3files")
            .WithReuse(true)
            .WithLabel("reuse-id", "infinilore-dev-s3files")
            .WithLabel("service", "minio")
            .WithLabel("com.docker.compose.project", ProjectLabel)
            .WithLabel("com.docker.compose.service", "minio")
            .WithLabel("com.docker.compose.version", "1.0")
            .WithLabel("com.docker.compose.container-number", "2")
            .WithLabel("com.docker.compose.depends_on", "");

        return new InfiniLoreContainers {
            SqlContainer = sqlBuilder.Build(),
            MinioContainer = minIoBuilder.Build()
        };
    }
    public static InfiniLoreContainers CreateForTesting() {
        // Configure an SQL Server container
        MsSqlBuilder sqlBuilder = new MsSqlBuilder()
            .WithLogger(Logger)
            .WithImage(SqlImage);

        // Configure a MinIO container
        MinioBuilder? minIoBuilder = new MinioBuilder()
            .WithLogger(Logger)
            .WithImage(MinioImage);

        return new InfiniLoreContainers {
            SqlContainer = sqlBuilder.Build(),
            MinioContainer = minIoBuilder.Build()
        };
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task InitializeAsync() {
        var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
        CancellationToken ct = cts.Token;
        try {
            Logger.LogInformation("Starting container initialization...");

            // Log before starting each container
            Logger.LogInformation("Initializing SQL Server container (this may take a while if the image needs to be downloaded)...");
            Task sqlTask = SqlContainer.StartAsync(ct);

            Logger.LogInformation("Initializing MinIO container (this may take a while if the image needs to be downloaded)...");
            Task minioTask = MinioContainer.StartAsync(ct);

            // Start both containers concurrently
            await Task.WhenAll(sqlTask, minioTask);

            Logger.LogInformation("All containers started successfully");
            Logger.LogInformation("SQL Connection string: {ConnectionString}", SqlContainer.GetConnectionString());
            Logger.LogInformation("MinIO Connection string: {ConnectionString}", MinioContainer.GetConnectionString());

        }
        catch (Exception ex) {
            Logger.LogError(ex, "Failed to initialize development environment");
            await DisposeAsync();
            throw;
        }
    }

    public async ValueTask DisposeAsync() {
        try {
            await Task.WhenAll(
                SqlContainer.DisposeAsync().AsTask(),
                MinioContainer.DisposeAsync().AsTask()
            );

            GC.SuppressFinalize(this);
        }
        catch (Exception ex) {
            Logger.LogError(ex, "Error during environment cleanup");
            throw;
        }
    }

    // ReSharper disable MemberCanBeMadeStatic.Global
    public string GetSqlConnectionString() => SqlContainer.GetConnectionString();
    public string GetMinioConnectionString() => MinioContainer.GetConnectionString();
    public string GetMinioAccessKey() => MinioAccessKey;
    public string GetMinioSecretKey() => MinioSecretKey;
    public string GetMinioPort() => MinioPort.ToString();
    // ReSharper enable MemberCanBeMadeStatic.Global
}
