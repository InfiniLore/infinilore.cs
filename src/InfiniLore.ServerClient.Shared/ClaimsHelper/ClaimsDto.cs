// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.ServerClient.Shared.ClaimsHelper;
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
        && Name.IsNotNullOrWhiteSpace() 
        && Email.IsNotNullOrWhiteSpace()
        && Roles.IsEmpty()    
    ;
}
