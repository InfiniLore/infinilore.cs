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
    [MaxLength(Defaults.UsernameMaxLength)] public string Username { get; set; } = string.Empty;
    
    public static class Defaults {
        public const int Auth0IdGoogleMaxLength = 256;
        public const int Auth0IdGithubMaxLength = 256;
        public const int UsernameMaxLength = 64;
    }
}
