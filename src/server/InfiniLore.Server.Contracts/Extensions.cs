// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics;

namespace InfiniLore.Server.Contracts;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Extensions {
    public static IQueryable<TSource> ConditionalQueryable<TSource>(this IQueryable<TSource> source, bool condition, Func<IQueryable<TSource>, IQueryable<TSource>> queryableFunc) =>
        condition
            ? queryableFunc(source)
            : source;
    
    public static Guid ToGuid(this string input) {
        #if DEBUG
        if (Guid.TryParse(input, out Guid output)) return output;
        Debug.Fail("Failed to parse Guid");
        return Guid.Empty;
        #else 
        // Because "testing" of the input is done during debug, we can just "blindly" parse during release.
        return Guid.Parse(input); 
        #endif
    }
}


// public class a {
//     public static void Main() {
//         Guid guid =  "12345678-1234-1234-1234-123456789012".ToGuid();
//     }
// }