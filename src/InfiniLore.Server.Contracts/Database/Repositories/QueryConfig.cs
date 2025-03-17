// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Server.Contracts.Database.Repositories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly record struct QueryConfig(
    bool AutoInclude = false,
    bool Reverse = false
);

// public readonly record struct QueryConfig<T>(
//     bool AutoInclude = false,
//     bool Reverse = false,
//     IQueryable<T>? Query = null
// ) {
//     public static implicit operator QueryConfig<T>(QueryConfig value) => new(
//         value.AutoInclude,
//         value.Reverse
//     );
// }

