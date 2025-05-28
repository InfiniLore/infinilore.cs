// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.DataModel.Response;
using Minio.DataModel.Result;
using System.Net;

namespace InfiniLore.Server.Modules.Core.S3;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IS3FileStorageService>]
public class MinIoS3FileStorageService(
    ILogger<MinIoS3FileStorageService> logger,
    IMinioClient minioClient
) : IS3FileStorageService {

    private static T GetArgs<T>(string bucketName) where T : BucketArgs<T>, new() => new T().WithBucket(bucketName);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<Result> CanConnectAsync(CancellationToken ct = default) {
        try {
            await minioClient.ListBucketsAsync(ct);
            return true;
        }
        catch (Exception e) {
            logger.Error(e, "Failed to connect to MinIO");
            return Result.FromError("Failed to connect to MinIO");
        }    
    }
    
    public async ValueTask<Result> TryInitializeBucketAsync(string bucketName, CancellationToken ct = default) {
        try {
            bool found = await minioClient.BucketExistsAsync(GetArgs<BucketExistsArgs>(bucketName), ct);
            if (found) return true;
            
            await minioClient.MakeBucketAsync(GetArgs<MakeBucketArgs>(bucketName), ct);
            return true;
        }
        catch (Exception e) {
            logger.Error(e, "Failed to initialize bucket {BucketName}", bucketName);
            return Result.FromError($"Failed to initialize bucket {bucketName}");
        }
    }
    
    public async ValueTask<Result> IsBucketInitializedAsync(string bucketName, CancellationToken ct = default) {
        try {
            bool found = await minioClient.BucketExistsAsync(GetArgs<BucketExistsArgs>(bucketName), ct);
            return found;
        }
        catch (Exception e) {
            logger.Error(e, "Failed to check if bucket {BucketName} is initialized", bucketName);
            return Result.FromError($"Failed to check if bucket {bucketName} is initialized");
        }
    }

    public async ValueTask<Result> TryDeleteBucketAsync(string bucketName, CancellationToken ct = default) {
        try {
            // First remove all objects
            ListObjectsArgs listArgs = new ListObjectsArgs()
                .WithBucket(bucketName)
                .WithRecursive(true);
            
            await foreach (Item item in minioClient.ListObjectsEnumAsync(listArgs, ct)) {
                await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(item.Key), ct);
            }

            // Then remove the bucket
            await minioClient.RemoveBucketAsync(GetArgs<RemoveBucketArgs>(bucketName), ct);
            return true;
        }
        catch (Exception e) {
            logger.Error(e, "Failed to delete bucket {BucketName}", bucketName);
            return Result.FromError($"Failed to delete bucket {bucketName}");
        }

    }
    public async ValueTask<Result<IReadOnlyList<string>>> TryListBucketsAsync(CancellationToken ct = default) {
        try {
            ListAllMyBucketsResult result = await minioClient.ListBucketsAsync(ct);
            return result.Buckets.Select(bucket => bucket.Name).ToList();
        }
        catch (Exception e) {
            logger.Error(e, "Failed to list buckets");
            return Result<IReadOnlyList<string>>.FromError("Failed to list buckets");
        }
    }

    public async ValueTask<Result> TryUploadFileAsync(string bucketName, string fileName, Stream fileData, string contentType, CancellationToken ct = default) {
        try {
            PutObjectArgs args = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithStreamData(fileData)
                .WithObjectSize(fileData.Length)
                .WithContentType(contentType);
            
            PutObjectResponse response = await minioClient.PutObjectAsync(args, ct);
            if (response.ResponseStatusCode == HttpStatusCode.OK) return true;
            return Result.FromError($"Failed to upload file {fileName} to bucket {bucketName}. Response status code: {response.ResponseStatusCode}");
        }
        catch (Exception e) {
            logger.Error(e, "Failed to upload file {FileName} to bucket {BucketName}", fileName, bucketName);
            return Result.FromError($"Failed to upload file {fileName} to bucket {bucketName}");
        }
    }

    public async ValueTask<Result> TryDownloadFileAsync(string bucketName, string fileName, Stream fileData, CancellationToken ct = default) {
        try {
            GetObjectArgs? args = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithCallbackStream(async (stream, token) => await stream.CopyToAsync(fileData, token));
            ObjectStat _ = await minioClient.GetObjectAsync(args, ct);
            return true;
        }
        catch (Exception e) {
            logger.Error(e, "Failed to download file {FileName} from bucket {BucketName}", fileName, bucketName);
            return Result.FromError($"Failed to download file {fileName} from bucket {bucketName}");
        }
    }
    
    public async ValueTask<Result> TryDeleteFileAsync(string bucketName, string fileName, CancellationToken ct = default) {
        try {
            RemoveObjectArgs args = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName);
            await minioClient.RemoveObjectAsync(args, ct);
            return true;
        }
        catch (Exception e) {
            logger.Error(e, "Failed to delete file {FileName} from bucket {BucketName}", fileName, bucketName);
            return Result.FromError($"Failed to delete file {fileName} from bucket {bucketName}");
        }
    }
    
    public async ValueTask<Result<IReadOnlyList<string>>> TryListFilesAsync(string bucketName, string prefix, CancellationToken ct = default) {
        try {
            ListObjectsArgs args = new ListObjectsArgs()
                .WithBucket(bucketName)
                .WithPrefix(prefix);
            
            var list = new List<string>();
            await foreach (Item item in minioClient.ListObjectsEnumAsync(args, ct)) {
                list.Add(item.Key);    
            }

            return list;
        }
        catch (Exception e) {
            logger.Error(e, "Failed to list files in bucket {BucketName}", bucketName);
            return Result<IReadOnlyList<string>>.FromError($"Failed to list files in bucket {bucketName}");       
        }
    }
}
