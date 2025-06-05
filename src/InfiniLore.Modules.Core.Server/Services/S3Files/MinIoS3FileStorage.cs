// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Modules.Core.Shared;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.DataModel.Response;
using Minio.DataModel.Result;
using System.Net;

namespace InfiniLore.Modules.Core.Server.S3Files;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IS3FileStorage>]
public class MinIoS3FileStorage(
    ILogger<MinIoS3FileStorage> logger,
    IMinioClient minioClient
) : IS3FileStorage {

    private readonly TimeSpan DefaultUrlExpiry = TimeSpan.FromMinutes(5);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<Outcome> CanConnectAsync(CancellationToken ct = default) {
        try {
            await minioClient.ListBucketsAsync(ct);
            return Outcome.True;
        }
        catch (Exception e) {
            logger.Error(e, "Failed to connect to MinIO");
            return Outcome.FromError("Failed to connect to MinIO");
        }    
    }
    
    public async ValueTask<Outcome> TryInitializeBucketAsync(string bucketName, CancellationToken ct = default) {
        try {
            bool found = await minioClient.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(bucketName),
                ct
            );
            if (found) return Outcome.True;
            
            await minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(bucketName),
                ct
            );
            return Outcome.True;
        }
        catch (Exception e) {
            logger.Error(e, "Failed to initialize bucket {BucketName}", bucketName);
            return Outcome.FromError($"Failed to initialize bucket {bucketName}");
        }
    }
    
    public async ValueTask<Outcome> IsBucketInitializedAsync(string bucketName, CancellationToken ct = default) {
        try {
            bool found = await minioClient.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(bucketName),
                ct
            );
            return Outcome.FromState(found);
        }
        catch (Exception e) {
            logger.Error(e, "Failed to check if bucket {BucketName} is initialized", bucketName);
            return Outcome.FromError($"Failed to check if bucket {bucketName} is initialized");
        }
    }

    public async ValueTask<Outcome> TryRemoveBucketAsync(string bucketName, CancellationToken ct = default) {
        try {
            // First, remove all objects
            ListObjectsArgs listArgs = new ListObjectsArgs()
                .WithBucket(bucketName)
                .WithRecursive(true);
            
            await foreach (Item item in minioClient.ListObjectsEnumAsync(listArgs, ct)) {
                await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(item.Key), ct);
            }

            // Then remove the bucket
            await minioClient.RemoveBucketAsync(
                new RemoveBucketArgs().WithBucket(bucketName),
                ct
            );
            return Outcome.True;
        }
        catch (Exception e) {
            logger.Error(e, "Failed to delete bucket {BucketName}", bucketName);
            return Outcome.FromError($"Failed to delete bucket {bucketName}");
        }

    }
    
    public async ValueTask<Outcome<IReadOnlyList<string>>> TryListBucketsAsync(CancellationToken ct = default) {
        try {
            ListAllMyBucketsResult result = await minioClient.ListBucketsAsync(ct);
            return result.Buckets.Select(bucket => bucket.Name).ToList();
        }
        catch (Exception e) {
            logger.Error(e, "Failed to list buckets");
            return Outcome<IReadOnlyList<string>>.FromError("Failed to list buckets");
        }
    }

    public async ValueTask<Outcome> TryUploadFileAsync(string bucketName, string fileName, Stream fileData, string contentType, CancellationToken ct = default) {
        try {
            await TryInitializeBucketAsync(bucketName, ct);
            
            PutObjectArgs args = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithStreamData(fileData)
                .WithObjectSize(fileData.Length)
                .WithContentType(contentType);
            
            PutObjectResponse response = await minioClient.PutObjectAsync(args, ct);
            if (response.ResponseStatusCode == HttpStatusCode.OK) return Outcome.True;
            return Outcome.FromError($"Failed to upload file {fileName} to bucket {bucketName}. Response status code: {response.ResponseStatusCode}");
        }
        catch (Exception e) {
            logger.Error(e, "Failed to upload file {FileName} to bucket {BucketName}", fileName, bucketName);
            return Outcome.FromError($"Failed to upload file {fileName} to bucket {bucketName}");
        }
    }

    public async ValueTask<Outcome> TryDownloadFileAsync(string bucketName, string fileName, Stream fileData, CancellationToken ct = default) {
        try {
            GetObjectArgs? args = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithCallbackStream(async (stream, token) => await stream.CopyToAsync(fileData, token));
            ObjectStat _ = await minioClient.GetObjectAsync(args, ct);
            return Outcome.True;
        }
        catch (Exception e) {
            logger.Error(e, "Failed to download file {FileName} from bucket {BucketName}", fileName, bucketName);
            return Outcome.FromError($"Failed to download file {fileName} from bucket {bucketName}");
        }
    }
    
    public async ValueTask<Outcome> TryRemoveFileAsync(string bucketName, string fileName, CancellationToken ct = default) {
        try {
            RemoveObjectArgs args = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName);
            await minioClient.RemoveObjectAsync(args, ct);
            return Outcome.True;
        }
        catch (Exception e) {
            logger.Error(e, "Failed to delete file {FileName} from bucket {BucketName}", fileName, bucketName);
            return Outcome.FromError($"Failed to delete file {fileName} from bucket {bucketName}");
        }
    }
    
    public async ValueTask<Outcome<IReadOnlyList<string>>> TryListFilesAsync(string bucketName, string prefix, CancellationToken ct = default) {
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
            return Outcome<IReadOnlyList<string>>.FromError($"Failed to list files in bucket {bucketName}");       
        }
    }
    
    public async ValueTask<Outcome<string>> GetFileUrlAsync(string bucketName, string fileName, TimeSpan? expiry = null, CancellationToken ct = default) {
        try {
            PresignedGetObjectArgs presignedArgs = new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithExpiry((int)(expiry ?? DefaultUrlExpiry).TotalSeconds); // Reduce expiry to 5 minutes
        
            string url = await minioClient.PresignedGetObjectAsync(presignedArgs);
            return Outcome<string>.FromData(url);
        }
        catch (Exception e) {
            logger.Error(e, "Failed to generate presigned URL for file {FileName} in bucket {BucketName}", fileName, bucketName);
            return Outcome<string>.FromError($"Failed to generate presigned URL for file {fileName} in bucket {bucketName}");       
        }
    }

}
