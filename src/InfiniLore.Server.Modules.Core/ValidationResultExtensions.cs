// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using FluentValidation.Results;

namespace InfiniLore.Server.Modules.Core;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ValidationResultExtensions {
    public static ProblemDetails ToProblemDetails(this ValidationResult validationResult) {
        // Create a new ProblemDetails instance
        var problemDetails = new ProblemDetails {
            Status = 400,
            Detail = "One or more validation errors occurred.",

            // Map each ValidationFailure in ValidationResult to ProblemDetails.Error
            Errors = validationResult.Errors
                .Select(failure => new ProblemDetails.Error {
                    Name = failure.PropertyName,// The name of the field or property
                    Reason = failure.ErrorMessage,// The validation error message
                    Code = failure.ErrorCode,// Optional error code (if configured in FluentValidation)
                    Severity = failure.Severity.ToString()// Error severity
                })
                .ToList()
        };

        return problemDetails;
    }
}
