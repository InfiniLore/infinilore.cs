// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Server.Database.Models.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniLoreUser : BasicData {
    [MaxLength(Defaults.Auth0IdGoogleMaxLength)] public string? Auth0IdGoogle { get; set; }
    [MaxLength(Defaults.Auth0IdGithubMaxLength)] public string? Auth0Github { get; set; }
    [MaxLength(Defaults.Auth0MailPasswordMaxLength)] public string? Auth0MailPassword { get; set; }
    [MaxLength(Defaults.UsernameMaxLength)] public string Username { get; set; } = string.Empty;

    // I have no clue why I didnt do this with a list
    public string[] GetAuth0Ids() => new[] { Auth0IdGoogle, Auth0Github, Auth0MailPassword }
        .Where(id => id is not null)!
        .ToArray<string>();

    public static class Defaults {
        public const int Auth0IdGoogleMaxLength = 256;
        public const int Auth0IdGithubMaxLength = 256;
        public const int Auth0MailPasswordMaxLength = 256;
        public const int UsernameMaxLength = 64;
    }
}
