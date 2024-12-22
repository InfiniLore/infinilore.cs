// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Account;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfiniLore.Database.Models;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Represents content created by a user within the InfiniLore database system. This class is abstract and
///     serves as a base for more specific types of content, providing common properties and behaviors associated with
///     user-generated data.
/// </summary>
public abstract class UserContent : BaseContent, IHasOwner {
    /// <summary>
    ///     Gets or sets the collection of access rights associated with the content.
    ///     This property contains a list of <see cref="UserContentAccessModel" /> objects that define
    ///     the access permissions granted to users for a specific content item.
    /// </summary>
    public ICollection<UserContentAccessModel> UserAccess { get; set; } = [];

    /// <summary>
    ///     Gets or sets a value indicating whether the content is publicly readable.
    ///     When authorization validation is performed, this property overrides the user's access level
    ///     to always allow for read access if set to true.
    /// </summary>
    public bool IsPubliclyReadable { get; set; } = false;

    /// <summary>
    ///     Gets or sets a value indicating whether the content is discoverable through search features.
    ///     When set to false, you should only be able to find this resource by direct references.
    /// </summary>
    public bool IsDiscoverable { get; set; } = true;

    /// <summary>
    ///     Determines whether the content should be included in discoverability search results.
    /// </summary>
    [NotMapped] public bool IncludeInDiscoverSearch => IsPubliclyReadable && IsDiscoverable;

    /// <summary>
    ///     Gets or sets the owner of the user content.
    /// </summary>
    public virtual InfiniLoreUser Owner { get; set; } = null!;
    public Guid OwnerId { get; set; }
}
