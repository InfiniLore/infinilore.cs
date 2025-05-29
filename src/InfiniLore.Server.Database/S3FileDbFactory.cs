// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace InfiniLore.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class S3FileDbFactory {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static void RegisterDatabase(
        IServiceCollection services,
        string connectionString,
        string accessKey,
        string secretKey
    ) {
        services.AddMinio(client => client.WithEndpoint(connectionString)
            .WithCredentials(accessKey, secretKey)
            .WithSSL(false) // For local development
        );
    }
}
