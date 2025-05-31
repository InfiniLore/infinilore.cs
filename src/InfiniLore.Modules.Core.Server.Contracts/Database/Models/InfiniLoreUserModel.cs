// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared.Database;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace InfiniLore.Modules.Core.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniLoreUserModel : BasicModel, IInfiniLoreUserModel {
    [MaxLength(Defaults.Auth0IdGoogleMaxLength)] public string? Auth0IdGoogle { get; set; }
    [MaxLength(Defaults.Auth0IdGithubMaxLength)] public string? Auth0Github { get; set; }
    [MaxLength(Defaults.Auth0MailPasswordMaxLength)] public string? Auth0MailPassword { get; set; }
    [MaxLength(Defaults.UsernameMaxLength)] public string Username { get; set; } = string.Empty;
    
    public Guid? ProfileImageMetaDataId { get; set; } 
    public S3FileMetaDataModel? ProfileImageMetaData { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public string? Auth0Id => Auth0IdGoogle ?? Auth0Github ?? Auth0MailPassword;

    // I have no clue why I didn't do this with a list
    public IEnumerable<string> GetAuth0Ids() => new[] { Auth0IdGoogle, Auth0Github, Auth0MailPassword }
        .Where(id => id is not null)!;

    // -----------------------------------------------------------------------------------------------------------------
    // Defaults
    // -----------------------------------------------------------------------------------------------------------------
    public static class Defaults {
        public const int Auth0IdGoogleMaxLength = 256;
        public const int Auth0IdGithubMaxLength = 256;
        public const int Auth0MailPasswordMaxLength = 256;
        public const int UsernameMaxLength = 64;
    }
}
