// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace Microsoft.Kiota.Abstractions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class RequestConfigurationExtensions {
    public static RequestConfiguration<T> AddJwtToken<T>(this RequestConfiguration<T> requestConfiguration, string token) where T : class, new() {
        
        requestConfiguration.Headers.Add("Authorization", $"Bearer {token}");
        
        return requestConfiguration;
    }
}
