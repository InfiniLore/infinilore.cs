// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FluentValidation.Results;
using JetBrains.Annotations;

namespace InfiniLore.Server.Modules.Core.Messaging;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ValidationResultExtensions {
    public static MessageResponse ToMediatorResponse(this ValidationResult validationResult) {
        string error = validationResult.Errors.Select(e => e.ErrorMessage).Aggregate((a, b) => a + ", " + b);
        return MessageResponse.FromErrorString(error);
    }

    // Used by ValidateRequestBehaviour
    [UsedImplicitly] public static MessageResponse<T> ToMediatorResponse<T>(this ValidationResult validationResult) {
        string error = validationResult.Errors.Select(e => e.ErrorMessage).Aggregate((a, b) => a + ", " + b);
        return MessageResponse<T>.FromErrorString(error);
    }
}
