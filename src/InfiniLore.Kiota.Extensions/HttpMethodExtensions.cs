// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Kiota.Abstractions;

namespace InfiniLore.Kiota;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class HttpMethodExtensions {
    public static string ToHttpMethodString(this Method method) => method  switch {
        Method.GET => "GET",
        Method.POST => "POST",
        Method.PATCH => "PATCH",
        Method.DELETE => "DELETE",
        Method.OPTIONS => "OPTIONS",
        Method.PUT => "PUT",
        Method.HEAD => "HEAD",
        Method.CONNECT => "CONNECT",
        Method.TRACE => "TRACE",
        _ => throw new ArgumentOutOfRangeException(nameof(method), method, null)
    };
}
