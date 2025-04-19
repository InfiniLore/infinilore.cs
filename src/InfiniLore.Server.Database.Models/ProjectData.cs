// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models.Data.User;
using System.Diagnostics.CodeAnalysis;

namespace InfiniLore.Server.Database.Models;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ProjectData : BasicData {
    public Guid LoreScopeId { get; set; } = Guid.Empty;
    [MaybeNull] public LoreScope LoreScope { get; set; } = null!;
}
