// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Shared.Services.ClaimsHelper;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IClaimsDto {
    string Auth0UserId { get; }
    string InfiniLoreUserId { get; }
    string InfiniloreUserName { get; }
    string Name { get; }
    string Email { get; }
    string[] Roles { get; }

    bool IsAuthenticated { get; }
    bool IsEmpty { get; }
}
