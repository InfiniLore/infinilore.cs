// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Account;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace InfiniLore.Database.Models.Content.Data.System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfinilorePermission : BaseContent {
    private string _name;
    [MaxLength(255)] public required string Name {
        get => _name;
        [MemberNotNull(nameof(_name))]
        set {
            _name = value;
            NormalizedName = value.ToUpperInvariant();
        }
    }

    [MaxLength(255)] public string NormalizedName { get; set; } = string.Empty;
    [MaxLength(511)] public string Description { get; set; } = string.Empty;
    
    public ICollection<InfiniLoreUser> Users { get; init; } = [];
}
