// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components.Forms;

namespace InfiniLore.Shared.Extensions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class FileStreamExtensions {
    public static async Task<MemoryStream> ToMemoryStreamAsync(
        this IBrowserFile file,
        long maxFileSize,
        CancellationToken ct = default
    ) {
        var memoryStream = new MemoryStream();
        await using Stream stream = file.OpenReadStream(maxFileSize, ct);
        await stream.CopyToAsync(memoryStream, ct);
        memoryStream.Position = 0;
        return memoryStream;
    }

    // Overload that accepts a buffer size parameter if needed
    public static async Task<MemoryStream> ToMemoryStreamAsync(
        this IBrowserFile file,
        long maxFileSize,
        int bufferSize,
        CancellationToken ct = default
    ) {
        var memoryStream = new MemoryStream();
        await using Stream stream = file.OpenReadStream(maxFileSize);
        await stream.CopyToAsync(memoryStream, bufferSize, ct);
        memoryStream.Position = 0;
        return memoryStream;
    }
}
