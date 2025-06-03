// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Server;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IS3FileStorage {
    ValueTask<Result> CanConnectAsync(CancellationToken ct = default);
    
    ValueTask<Result> TryInitializeBucketAsync(string bucketName, CancellationToken ct = default);
    ValueTask<Result> IsBucketInitializedAsync(string bucketName, CancellationToken ct = default);
    ValueTask<Result> TryRemoveBucketAsync(string bucketName, CancellationToken ct = default);
    
    ValueTask<Result<IReadOnlyList<string>>> TryListBucketsAsync(CancellationToken ct = default);
    
    ValueTask<Result> TryUploadFileAsync(string bucketName, string fileName, Stream fileData, string contentType, CancellationToken ct = default);
    ValueTask<Result> TryDownloadFileAsync(string bucketName, string fileName, Stream fileData, CancellationToken ct = default);
    ValueTask<Result> TryRemoveFileAsync(string bucketName, string fileName, CancellationToken ct = default);
    ValueTask<Result<IReadOnlyList<string>>> TryListFilesAsync(string bucketName, string filePath, CancellationToken ct = default);
    ValueTask<Result<string>> GetFileUrlAsync(string bucketName, string fileName, TimeSpan? expiry = null, CancellationToken ct = default);
}
