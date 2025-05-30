// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfiniLore.Modules.Core.Server.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class S3FileMetaDataModel : BasicModel {
    [MaxLength(Defaults.MaxFileNameLength)] public required string FileName { get; set; }
    [MaxLength(Defaults.MaxContentTypeLength)] public required string ContentType { get; set; }
    [NotMapped] public Stream? DataStream { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Defaults
    // -----------------------------------------------------------------------------------------------------------------
    public static class Defaults {
        public const int MaxFileNameLength = 256;
        public const int MaxContentTypeLength = 256;
    }
}
