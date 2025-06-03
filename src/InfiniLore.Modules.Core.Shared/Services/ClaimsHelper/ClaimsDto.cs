// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared.ClaimsHelper;

namespace InfiniLore.Modules.Core.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record ClaimsDto(
    string Auth0UserId,
    string InfiniLoreUserId,
    string InfiniloreUserName,
    string Name,
    string Email,
    string[] Roles
) : IClaimsDto {

    public static ClaimsDto Empty => new(
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        []
    ) {
        IsAuthenticated = false
    };

    public bool IsAuthenticated { get; private init; } = true;
    public bool IsEmpty =>
        Auth0UserId.IsNotNullOrWhiteSpace()
        && InfiniLoreUserId.IsNotNullOrWhiteSpace()
        && InfiniloreUserName.IsNotNullOrWhiteSpace()
        && Name.IsNotNullOrWhiteSpace()
        && Email.IsNotNullOrWhiteSpace()
        && Roles.IsEmpty();
}
