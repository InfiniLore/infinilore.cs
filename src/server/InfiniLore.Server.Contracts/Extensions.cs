// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Server.Contracts;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Extensions {
    public static IQueryable<TSource> ConditionalQueryable<TSource>(this IQueryable<TSource> source, bool condition, Func<IQueryable<TSource>, IQueryable<TSource>> queryableFunc) =>
        condition
            ? queryableFunc(source)
            : source;
}
