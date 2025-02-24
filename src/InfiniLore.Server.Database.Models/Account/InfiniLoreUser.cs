// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Server.Database.Models.Account;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniLoreUser : BasicData {
    [MaxLength(256)] public string? Auth0IdGoogle { get; set; }
    [MaxLength(256)] public string? Auth0Github { get; set; }
    [MaxLength(256)] public string Username { get; set; } = string.Empty;
}
