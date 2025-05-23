// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Shared;
using InfiniLore.Shared.Modules.LoreScopes.Database;
using System.Text.Json;

namespace InfiniLore.Kiota.Extensions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class KiotaMapper {
    public static PaginatedResult<T> MapToPaginatedResult<T>(object? kiotaResponse) where T : notnull {
        if (kiotaResponse is null)
            return PaginatedResult<T>.FromError("Received null response from API");

        try {
            string jsonString = JsonSerializer.Serialize(kiotaResponse);
            var intermediateData = JsonSerializer.Deserialize<KiotaResponseWrapper>(jsonString);

            if (intermediateData?.Items is null)
                return PaginatedResult<T>.FromError("Invalid response format");

            var paginatedData = new PaginatedData<T>(
                intermediateData.Items.Cast<T>().ToArray(),
                intermediateData.TotalCount,
                intermediateData.CurrentPage,
                intermediateData.TotalPages
            );

            return PaginatedResult<T>.FromData(paginatedData);
        }
        catch (Exception ex) {
            return PaginatedResult<T>.FromError($"Failed to map response: {ex.Message}");
        }
    }

    private class KiotaResponseWrapper {
        public object[]? Items { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
