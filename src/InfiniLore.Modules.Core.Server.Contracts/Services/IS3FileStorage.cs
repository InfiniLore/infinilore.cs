// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared;

namespace InfiniLore.Modules.Core.Server;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IS3FileStorage {
    ValueTask<Outcome> CanConnectAsync(CancellationToken ct = default);
    
    ValueTask<Outcome> TryInitializeBucketAsync(string bucketName, CancellationToken ct = default);
    ValueTask<Outcome> IsBucketInitializedAsync(string bucketName, CancellationToken ct = default);
    ValueTask<Outcome> TryRemoveBucketAsync(string bucketName, CancellationToken ct = default);
    
    ValueTask<Outcome<IReadOnlyList<string>>> TryListBucketsAsync(CancellationToken ct = default);
    
    ValueTask<Outcome> TryUploadFileAsync(string bucketName, string fileName, Stream fileData, string contentType, CancellationToken ct = default);
    ValueTask<Outcome> TryDownloadFileAsync(string bucketName, string fileName, Stream fileData, CancellationToken ct = default);
    ValueTask<Outcome> TryRemoveFileAsync(string bucketName, string fileName, CancellationToken ct = default);
    ValueTask<Outcome<IReadOnlyList<string>>> TryListFilesAsync(string bucketName, string filePath, CancellationToken ct = default);
    ValueTask<Outcome<string>> GetFileUrlAsync(string bucketName, string fileName, TimeSpan? expiry = null, CancellationToken ct = default);
}
