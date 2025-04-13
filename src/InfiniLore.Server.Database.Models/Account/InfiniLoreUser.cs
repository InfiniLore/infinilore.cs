// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Server.Database.Models.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// A user within <see cref="InfiniLore"/>.
/// </summary>
public class InfiniLoreUser : BasicData {
    [MaxLength(Defaults.Auth0IdGoogleMaxLength)] public string? Auth0IdGoogle { get; set; }
    [MaxLength(Defaults.Auth0IdGithubMaxLength)] public string? Auth0Github { get; set; }
    [MaxLength(Defaults.Auth0MailPasswordMaxLength)] public string? Auth0MailPassword { get; set; }
    /// <summary>
    /// This user's unique username.
    /// </summary>
    /// <remarks>
    /// By default is <see cref="string.Empty"/>
    /// </remarks>
    [MaxLength(Defaults.UsernameMaxLength)] public string Username { get; set; } = string.Empty;

    // I have no clue why I didn't do this with a list
    /// <returns>a list of <see href="https://auth0.com/">Auth0</see> ids.
    /// 
    /// <para>
    /// The ordering of the returned ids is:
    /// <list type="number">
    /// <item>
    /// <see cref="Auth0IdGoogle"/>
    /// </item>
    /// <item>
    /// <see cref="Auth0Github"/>
    /// </item>
    /// <item>
    /// <see cref="Auth0MailPassword"/>
    /// </item>
    /// </list>
    /// </para>
    /// </returns>
    public string[] GetAuth0Ids() => new[] { Auth0IdGoogle, Auth0Github, Auth0MailPassword }
        .Where(id => id is not null)!
        .ToArray<string>();

    public static class Defaults {
        public const int Auth0IdGoogleMaxLength = 256;
        public const int Auth0IdGithubMaxLength = 256;
        public const int Auth0MailPasswordMaxLength = 256;
        /// <summary>
        /// The maximum possible length for a <see cref="InfiniLoreUser"/>'s <see cref="InfiniLoreUser.Username"/> can be.
        /// </summary>
        public const int UsernameMaxLength = 64;
    }
}
