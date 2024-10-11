// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Server.Data.Models.Account;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class JwtRefreshToken {
    // Not part of the regular UserContent flow because it isn't able to be soft-deleted.
    // It's a one-time use token that is used to refresh the JWT.
    [Key] public int Id { get; init; }
    [MaxLength(64)]public required string TokenHash { get; init; } 
    public required DateTime ExpiresAt { get; init; }
    
    public required InfiniLoreUser User { get; set; }
    [MaxLength(48)] public string UserId { get; set; } = null!;
    
}
