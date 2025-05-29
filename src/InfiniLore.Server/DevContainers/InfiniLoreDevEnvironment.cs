// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;
using Serilog;
using Testcontainers.Minio;
using Testcontainers.MsSql;
using ILogger=Microsoft.Extensions.Logging.ILogger;

namespace InfiniLore.Server.DevContainers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniLoreDevEnvironment : IAsyncDisposable {
    private readonly INetwork _network;
    private readonly MsSqlContainer _sqlContainer;
    private readonly MinioContainer _minioContainer;
    
    private static readonly ILoggerFactory EmptyLoggerFactory = LoggerFactory.Create(builder => builder.AddSerilog(Log.Logger));
    private readonly ILogger _logger = EmptyLoggerFactory.CreateLogger("DOCKER-DEV-ENV");
    
    private const int SqlPort = 40626;
    private const string SqlPassword = "AnnaIsTrans4Ever!";
    
    private const int MinioPort = 40627;
    private const string MinioAccessKey = "minioadmin";
    private const string MinioSecretKey = "minioadmin";

    private static string GetNetworkName(bool isTesting) => $"infinilore-{(isTesting ? "test" : "dev")}-network";
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public InfiniLoreDevEnvironment(bool isTesting = false) {
        // Create a shared network
        _network = new NetworkBuilder()
            .WithName(GetNetworkName(isTesting))
            .Build();

        // Configure an SQL Server container
        MsSqlBuilder sqlBuilder = new MsSqlBuilder()
            .WithPortBinding(SqlPort, MsSqlBuilder.MsSqlPort)
            .WithNetwork(_network)
            .WithLogger(_logger)
            .WithImage("mcr.microsoft.com/mssql/server:2022-CU10-ubuntu-22.04");

        if (!isTesting) {
            sqlBuilder = sqlBuilder
                .WithPassword(SqlPassword)
                .WithName("infinilore-dev-content")
                .WithReuse(true)
                .WithLabel("reuse-id", "infinilore-dev-content");
        }
            
        _sqlContainer = sqlBuilder.Build();

        // Configure a MinIO container
        MinioBuilder? minIoBuilder = new MinioBuilder()
            .WithPortBinding(MinioPort, MinioBuilder.MinioPort)
            .WithNetwork(_network)
            .WithLogger(_logger)
            .WithImage("minio/minio");
        
        if (!isTesting) {
            minIoBuilder = minIoBuilder
                .WithName("infinilore-dev-file")
                .WithReuse(true)
                .WithLabel("reuse-id", "infinilore-dev-file")
                .WithEnvironment("MINIO_ROOT_USER", MinioAccessKey)
                .WithEnvironment("MINIO_ROOT_PASSWORD", MinioSecretKey);
        }
        
        _minioContainer = minIoBuilder.Build();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task InitializeAsync() {
        try {
            await _network.CreateAsync();

            // Start both containers concurrently
            await Task.WhenAll(
                _sqlContainer.StartAsync(),
                _minioContainer.StartAsync()
            );

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

            await _network.DisposeAsync();
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
