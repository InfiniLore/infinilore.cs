// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Server;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IS3FileStorage {
    ValueTask<Shared.Outcome> CanConnectAsync(CancellationToken ct = default);
    
    ValueTask<Shared.Outcome> TryInitializeBucketAsync(string bucketName, CancellationToken ct = default);
    ValueTask<Shared.Outcome> IsBucketInitializedAsync(string bucketName, CancellationToken ct = default);
    ValueTask<Shared.Outcome> TryRemoveBucketAsync(string bucketName, CancellationToken ct = default);
    
    ValueTask<Shared.Outcome<IReadOnlyList<string>>> TryListBucketsAsync(CancellationToken ct = default);
    
    ValueTask<Shared.Outcome> TryUploadFileAsync(string bucketName, string fileName, Stream fileData, string contentType, CancellationToken ct = default);
    ValueTask<Shared.Outcome> TryDownloadFileAsync(string bucketName, string fileName, Stream fileData, CancellationToken ct = default);
    ValueTask<Shared.Outcome> TryRemoveFileAsync(string bucketName, string fileName, CancellationToken ct = default);
    ValueTask<Shared.Outcome<IReadOnlyList<string>>> TryListFilesAsync(string bucketName, string filePath, CancellationToken ct = default);
    ValueTask<Shared.Outcome<string>> GetFileUrlAsync(string bucketName, string fileName, TimeSpan? expiry = null, CancellationToken ct = default);
}
