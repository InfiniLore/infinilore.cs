// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Models.Account;
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Server.Data.Models.Base;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class UserContent<T> : BaseContent<T> where T : BaseContent<T> {
    public required InfiniLoreUser User { get; set; }
    [MaxLength(48)] public string UserId { get; set; } = null!;
}