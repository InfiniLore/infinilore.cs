// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.ServerClient.Shared.Auth0;

namespace InfiniLore.ServerClient.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record Auth0Information(
    string UserId,
    string Name,
    string Email
) : IAuth0Information {

    public static Auth0Information Empty => new(string.Empty, string.Empty, string.Empty) {
        IsAuthenticated = false
    };
    public bool IsAuthenticated { get; private init; } = true;
    public bool IsEmpty => UserId.IsNotNullOrWhiteSpace() && Name.IsNotNullOrWhiteSpace() && Email.IsNotNullOrWhiteSpace();
}
