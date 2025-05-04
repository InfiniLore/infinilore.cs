// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class RepositoryFailures {
    public const string Unknown = nameof(Unknown);
    public const string ModelFailedValidation = nameof(ModelFailedValidation);
    public const string ModelFailedUniqueConstraint = nameof(ModelFailedUniqueConstraint);
    public const string ModelNotFound = nameof(ModelNotFound);
    public const string ModelsNotFound = nameof(ModelsNotFound);
    public const string PaginationInvalidPageNumber = nameof(PaginationInvalidPageNumber);
    public const string PaginationInvalidPageSize = nameof(PaginationInvalidPageSize);
}
