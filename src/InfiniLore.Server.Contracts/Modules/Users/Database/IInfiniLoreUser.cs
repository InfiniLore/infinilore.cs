// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Modules.Users.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IInfiniLoreUser : IBasicData {
    string? Auth0IdGoogle { get; set; }
    string? Auth0Github { get; set; }
    string? Auth0MailPassword { get; set; }
    string Username { get; set; } = string.Empty;

    public string[] GetAuth0Ids();
}
