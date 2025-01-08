// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace InfiniLore.Database.Models.Content.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LorescopeModel : UserContent {
    [MaxLength(64)] public required string Name { get; set; }
    [MaxLength(512)] public string Description { get; set; } = string.Empty;

    public ICollection<MultiverseModel> Multiverses { get; init; } = [];

    [NotMapped] [MemberNotNullWhen(true, nameof(SingleMultiverse))] public bool HasOnlyOneMultiverse => Multiverses.Count == 1;
    [NotMapped] public MultiverseModel? SingleMultiverse => HasOnlyOneMultiverse == false ? null : Multiverses.First();
    
    public bool TryGetAsSingleMultiverse([NotNullWhen(true)] out MultiverseModel? value) {
        value = HasOnlyOneMultiverse ? SingleMultiverse : null;
        return HasOnlyOneMultiverse;
    }
}
