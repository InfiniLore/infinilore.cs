// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Shared.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class DefaultInfiniLoreUserModel : IInfiniLoreUserModel {
    public Guid Id { get; } = Guid.Empty;
    public string Username { get; } = "Unknown";
    public string? Auth0Id { get; } = null ;
    public string? ProfileImageUrl { get; }= null ;
}
