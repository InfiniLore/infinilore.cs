// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Server.Contracts.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public enum RepositoryFailures {
    Unknown = 0,
    
    ModelFailedValidation,
    ModelFailedUniqueConstraint,
    
    ModelNotFound,
    ModelsNotFound,
    
    PaginationInvalidPageNumber,
    PaginationInvalidPageSize,
}
