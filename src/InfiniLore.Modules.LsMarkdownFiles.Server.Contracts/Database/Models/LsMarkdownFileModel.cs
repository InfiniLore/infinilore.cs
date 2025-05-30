// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Database.RepoMethods;
using InfiniLore.Modules.Core.Shared.Database;
using InfiniLore.Modules.LsMarkdownFiles.Shared.Database;
using InfiniLore.Server.Modules.LoreScopes.Database;
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Server.Modules.LsMarkdownFiles.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LsMarkdownFileModel : OwnedModel<LoreScopeModel>, ILsMarkdownFileModel, IHasName {
    [MaxLength(Defaults.NameMaxLength)] public required string Name { get; set; } = null!;
    public required Guid LastEditor { get; set; }
    
    public required Guid S3FileMetaDataId { get; set; } 
    public S3FileMetaDataModel? S3FileMetaData { get; set; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Default
    // -----------------------------------------------------------------------------------------------------------------
    public static class Defaults {
        public const int NameMaxLength = 100;
    }
}
