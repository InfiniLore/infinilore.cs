// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Server.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly record struct QueryConfig(
    bool OptionalInclude = false,
    bool Reverse = false,
    bool RetrieveSoftDeleted = false
) {
    public static QueryConfig Default => new();
    public static QueryConfig WithOptional => new(OptionalInclude: true);
    public static QueryConfig WithReverse => new(Reverse: true);
    public static QueryConfig WithRetrieveSoftDeleted => new(RetrieveSoftDeleted: true);
}