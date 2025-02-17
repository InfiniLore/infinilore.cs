// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Server.Database.Models.Account;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class JwtRefreshTokenData : UserData {
    public const int MaxLengthTokenHash = 255;
    [MaxLength(MaxLengthTokenHash)] public required string TokenHash { get; init; } = string.Empty;
    public required int ExpiresInDays { get; init; } = 0;

    public string[] Roles { get; init; } = [];
    public string[] Permissions { get; init; } = [];
    
    public DateOnly ExpiresAt => DateOnly.FromDateTime(CreatedDate.AddDays(ExpiresInDays));
}
