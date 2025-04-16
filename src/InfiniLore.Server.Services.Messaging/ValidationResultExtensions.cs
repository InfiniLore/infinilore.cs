// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FluentValidation.Results;
using JetBrains.Annotations;

namespace InfiniLore.Server.Services.Messaging;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ValidationResultExtensions {
    public static MediatorResponse ToMediatorResponse(this ValidationResult validationResult) {
        string error = validationResult.Errors.Select(e => e.ErrorMessage).Aggregate((a, b) => a + ", " + b);
        return MediatorResponse.FromErrorString(error);
    }

    // Used by ValidateRequestBehaviour
    [UsedImplicitly] public static MediatorResponse<T> ToMediatorResponse<T>(this ValidationResult validationResult) {
        string error = validationResult.Errors.Select(e => e.ErrorMessage).Aggregate((a, b) => a + ", " + b);
        return MediatorResponse<T>.FromErrorString(error);
    }
}
