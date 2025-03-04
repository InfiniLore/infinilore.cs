// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.ServerClient.Shared;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record Auth0Information(
    string UserId,
    string Name,
    string Email
) : IAuth0Information {
    public bool IsAuthenticated { get; private init; } = true;
    public bool IsEmpty => UserId.IsNotNullOrWhiteSpace() && Name.IsNotNullOrWhiteSpace() && Email.IsNotNullOrWhiteSpace();
    
    public static Auth0Information Empty => new(string.Empty, string.Empty, string.Empty) {
        IsAuthenticated = false
    };
}
