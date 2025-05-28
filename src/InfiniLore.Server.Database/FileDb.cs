// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Minio;
using Minio.DataModel.Args;

namespace InfiniLore.Server.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MinIoFileDb(IMinioClient minioClient) {
    public async Task Example() {
        bool found = await minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket("testing"));
        if (!found) await minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket("testing"));

        await using FileStream fileStream = File.OpenRead(@"E:\Portfolio\internal\006-infinilore\0001-cs-infinilore\src\InfiniLore.Server\wwwroot\paper-texture.jpg");
        await minioClient.PutObjectAsync(new PutObjectArgs()
            .WithBucket("testing")
            .WithObject("paper-texture.jpg")
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
        );
    }
}
