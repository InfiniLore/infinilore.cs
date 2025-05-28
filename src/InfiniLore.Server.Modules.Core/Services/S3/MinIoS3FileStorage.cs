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
[InjectableScoped<IS3FileStorage>]
public class MinIoS3FileStorage(
    ILogger<MinIoS3FileStorage> logger,
    IMinioClient minioClient
) : IS3FileStorage {
    
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
            bool found = await minioClient.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(bucketName),
                ct
            );
            if (found) return true;
            
            await minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(bucketName),
                ct
            );
            return true;
        }
        catch (Exception e) {
            logger.Error(e, "Failed to initialize bucket {BucketName}", bucketName);
            return Result.FromError($"Failed to initialize bucket {bucketName}");
        }
    }
    
    public async ValueTask<Result> IsBucketInitializedAsync(string bucketName, CancellationToken ct = default) {
        try {
            bool found = await minioClient.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(bucketName),
                ct
            );
            return found;
        }
        catch (Exception e) {
            logger.Error(e, "Failed to check if bucket {BucketName} is initialized", bucketName);
            return Result.FromError($"Failed to check if bucket {bucketName} is initialized");
        }
    }

    public async ValueTask<Result> TryDeleteBucketAsync(string bucketName, CancellationToken ct = default) {
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

    public async ValueTask<Result> TryUploadFileAsync(string bucketName, Union<Guid, string> fileName, Stream fileData, string contentType, CancellationToken ct = default) {
        try {
            await TryInitializeBucketAsync(bucketName, ct);
            
            string fileNameCorrected = fileName.Match(guid => guid.ToString(), str => str);
            PutObjectArgs args = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileNameCorrected)
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

    public async ValueTask<Result> TryDownloadFileAsync(string bucketName, Union<Guid, string> fileName, Stream fileData, CancellationToken ct = default) {
        try {
            string fileNameCorrected = fileName.Match(guid => guid.ToString(), str => str);
            GetObjectArgs? args = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileNameCorrected)
                .WithCallbackStream(async (stream, token) => await stream.CopyToAsync(fileData, token));
            ObjectStat _ = await minioClient.GetObjectAsync(args, ct);
            return true;
        }
        catch (Exception e) {
            logger.Error(e, "Failed to download file {FileName} from bucket {BucketName}", fileName, bucketName);
            return Result.FromError($"Failed to download file {fileName} from bucket {bucketName}");
        }
    }
    
    public async ValueTask<Result> TryDeleteFileAsync(string bucketName, Union<Guid, string> fileName, CancellationToken ct = default) {
        try {
            string fileNameCorrected = fileName.Match(guid => guid.ToString(), str => str);
            RemoveObjectArgs args = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileNameCorrected);
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
    
    public async ValueTask<Result<string>> GetFileUrlAsync(string bucketName, Union<Guid, string> fileName, CancellationToken ct) {
        try {
            PresignedGetObjectArgs? presignedArgs = new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName.Match(
                    guid => guid.ToString(),
                    str => str
                ))
                .WithExpiry(60 * 60 * 24); // 24 hours expiry

            return await minioClient.PresignedGetObjectAsync(presignedArgs);
        }
        
        catch (Exception e) {
            logger.Error(e, "Failed to generate presigned URL for file {FileName} in bucket {BucketName}", fileName, bucketName);
            return Result<string>.FromError($"Failed to generate presigned URL for file {fileName} in bucket {bucketName}");       
        }
    }

}
