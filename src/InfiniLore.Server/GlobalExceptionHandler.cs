// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Docker.DotNet;
using Serilog;
using System.Text.RegularExpressions;

namespace InfiniLore.Server;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static partial class GlobalExceptionHandler {

    [GeneratedRegex(@"ServiceType:\s([^\s]+)\sLifetime:\s([^\s]+)\sImplementationType:\s([^\s]+)'(?:.*)type\s'([^\s]+)'")]
    private static partial Regex InvalidOperationExceptionForServiceNotImplementedRegex { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static async Task<int> ExecuteWithGlobalExceptionHandlingAsync(Func<Task<int>> action) {
        try {
            return await action.Invoke();
        }
        catch (DockerApiException ex) {
            Log.Logger.Fatal(ex, "Docker API Exception \"{ExceptionType}\"", ex.GetType());
            return ex.HResult;
        }
        catch (AggregateException ex) {
            HandleAggregateException(ex);
            return ex.HResult;
        }
        catch (Exception ex) {
            Log.Logger.Fatal(ex, "Host terminated unexpectedly \"{ExceptionType}\"", ex.GetType());
            return ex.HResult;
        }
        finally {
            await Log.CloseAndFlushAsync();
        }
    }

    private static void HandleAggregateException(AggregateException aggregateException) {
        foreach (Exception innerException in aggregateException.InnerExceptions) {
            switch (innerException) {
                // Example handling for dependency-injection-related InvalidOperationException
                case InvalidOperationException { Source: "Microsoft.Extensions.DependencyInjection" } invalidOperationException: {
                    if (TryHandleInvalidOperationException(invalidOperationException)) break;

                    goto default;
                }

                default: {
                    Log.Logger.Fatal(innerException, "Host terminated unexpectedly \"{ExceptionType}\": {Message}",
                        innerException.GetType(), innerException.Message);

                    break;
                }
            }
        }
    }

    private static bool TryHandleInvalidOperationException(InvalidOperationException exception) {
        Match match = InvalidOperationExceptionForServiceNotImplementedRegex.Match(exception.Message);
        if (match.Groups is not { Count: > 0 } groups) return false;

        string serviceType = groups[1].Value;
        string lifetime = groups[2].Value;
        string implementationType = groups[3].Value;
        string notFoundService = groups[4].Value;

        Log.Logger.Fatal(
            exception,
            "Dependent service {NotFoundService} not found for ServiceType: {ServiceType} [{Lifetime}] with ImplementationType: {ImplementationType}",
            notFoundService,
            serviceType,
            lifetime,
            implementationType
        );

        return true;
    }
}
