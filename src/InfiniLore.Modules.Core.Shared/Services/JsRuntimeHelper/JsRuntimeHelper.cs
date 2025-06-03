// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Core.Shared;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IJsRuntimeHelper>]
public class JsRuntimeHelper(
    IJsSecureStorage jsSecureStorage
) : IJsRuntimeHelper {

    public IJsSecureStorage SecureStorage { get; } = jsSecureStorage;
}
