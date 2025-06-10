// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IJsRuntimeHelper {
    IJsSecureStorage SecureStorage { get; }

    ValueTask WriteToClipboardAsync(string text);

    ValueTask StoreToLocalStorageAsync<T>(string key, T value);
    ValueTask<T?> GetFromLocalStorageAsync<T>(string key);
    ValueTask RemoveFromLocalStorageAsync(string key);
}