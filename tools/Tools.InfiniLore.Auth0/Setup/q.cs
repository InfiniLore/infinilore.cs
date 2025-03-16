// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Auth0.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Credentials.Auth0;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class RateLimitHelper {
    public static async Task RetryWithRateLimit(Func<Task> action, int maxRetries = 5, int backoffSeconds = 2, ILogger? logger = null) {
        int retryCounter = 0;

        while (retryCounter < maxRetries) {
            try {
                await action();
                return;
            } catch (RateLimitApiException ex) {
                if (retryCounter >= maxRetries - 1) {
                    // Log the error and rethrow the exception if retries are exhausted
                    logger?.LogError(ex, "Rate limit reached. Max retries reached.");
                    throw;
                }

                retryCounter++;
                int delay = backoffSeconds * retryCounter;
                // Log the error and wait for the specified delay
                logger?.LogError(ex, $"Rate limit reached. Retrying in {delay} seconds... Attempt {retryCounter}/{maxRetries}");
                await Task.Delay(TimeSpan.FromSeconds(delay));
            }
        }
    }

}
