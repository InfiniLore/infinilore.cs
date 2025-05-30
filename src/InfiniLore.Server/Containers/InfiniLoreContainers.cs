// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Serilog;
using Testcontainers.Minio;
using Testcontainers.MsSql;
using ILogger=Microsoft.Extensions.Logging.ILogger;

namespace InfiniLore.Server.Containers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniLoreContainers : IAsyncDisposable {
    private readonly MsSqlContainer _sqlContainer;
    private readonly MinioContainer _minioContainer;

    private static readonly ILoggerFactory EmptyLoggerFactory = LoggerFactory.Create(builder => builder.AddSerilog(Log.Logger));
    private readonly ILogger _logger = EmptyLoggerFactory.CreateLogger("DOCKER-DEV-ENV");

    private const int SqlPort = 40626;
    private const string SqlPassword = "AnnaIsTrans4Ever!";

    private const int MinioPort = 40627;
    private const string MinioAccessKey = "minioadmin";
    private const string MinioSecretKey = "minioadmin";

    private const string ProjectLabel = "infinilore-dev";

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public InfiniLoreContainers(bool isTesting = false) {
        // Configure an SQL Server container
        MsSqlBuilder sqlBuilder = new MsSqlBuilder()
            .WithPortBinding(SqlPort, MsSqlBuilder.MsSqlPort)
            .WithLogger(_logger)
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest");

        if (!isTesting) {
            sqlBuilder = sqlBuilder
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
        }

        _sqlContainer = sqlBuilder.Build();

        // Configure a MinIO container
        MinioBuilder? minIoBuilder = new MinioBuilder()
            .WithPortBinding(MinioPort, MinioBuilder.MinioPort)
            .WithLogger(_logger)
            .WithImage("minio/minio:latest");

        if (!isTesting) {
            minIoBuilder = minIoBuilder
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
        }

        _minioContainer = minIoBuilder.Build();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task InitializeAsync() {
        var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
        CancellationToken ct = cts.Token;
        try {
            _logger.LogInformation("Starting container initialization...");

            // Log before starting each container
            _logger.LogInformation("Initializing SQL Server container (this may take a while if the image needs to be downloaded)...");
            Task sqlTask = _sqlContainer.StartAsync(ct);

            _logger.LogInformation("Initializing MinIO container (this may take a while if the image needs to be downloaded)...");
            Task minioTask = _minioContainer.StartAsync(ct);

            // Start both containers concurrently
            await Task.WhenAll(sqlTask, minioTask);

            _logger.LogInformation("All containers started successfully");
            _logger.LogInformation("SQL Connection string: {ConnectionString}", _sqlContainer.GetConnectionString());
            _logger.LogInformation("MinIO Connection string: {ConnectionString}", _minioContainer.GetConnectionString());

        }
        catch (Exception ex) {
            _logger.LogError(ex, "Failed to initialize development environment");
            await DisposeAsync();
            throw;
        }
    }

    public async ValueTask DisposeAsync() {
        try {
            await Task.WhenAll(
                _sqlContainer.DisposeAsync().AsTask(),
                _minioContainer.DisposeAsync().AsTask()
            );

            GC.SuppressFinalize(this);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error during environment cleanup");
            throw;
        }
    }

    // ReSharper disable MemberCanBeMadeStatic.Global
    public string GetSqlConnectionString() => _sqlContainer.GetConnectionString();
    public string GetMinioConnectionString() => _minioContainer.GetConnectionString();
    public string GetMinioAccessKey() => MinioAccessKey;
    public string GetMinioSecretKey() => MinioSecretKey;
    public string GetMinioPort() => MinioPort.ToString();
    // ReSharper enable MemberCanBeMadeStatic.Global
}
