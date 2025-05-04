// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Database.Models;

namespace InfiniLore.Server.Modules.Users.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IInfiniLoreUserAuth0Data : IBasicData {
    string? Auth0IdGoogle { get; set; }
    string? Auth0Github { get; set; }
    string? Auth0MailPassword { get; set; }

    public string[] GetAuth0Ids();
}
