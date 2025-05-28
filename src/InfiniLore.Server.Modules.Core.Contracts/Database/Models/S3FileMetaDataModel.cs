// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations.Schema;

namespace InfiniLore.Server.Modules.Core.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class S3FileMetaDataModel : BasicModel {
    public required string FileName { get; set; }
    public required string ContentType { get; set; }
    
    [NotMapped] public Stream? DataStream { get; set; }
}
