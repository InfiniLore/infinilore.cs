// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Flags]
public enum QueryConfig {
    None                      = 0b0,
    
    Reversed                  = 0b1,
    IncludeSoftDeleted        = 0b10,
    IncludeOptionalReferences = 0b100,
    SortByCreatedAt           = 0b1000,
    SortByModifiedAt          = 0b10000,
}

public static class QueryConfigExtensions {
    public static bool HasFlagFast(this QueryConfig value, QueryConfig flag) {
        return (value & flag) != 0;
    }
}
