// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components.Web;

namespace InfiniLore.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class RenderModes {
    public static readonly InteractiveWebAssemblyRenderMode WebAssemblyOnly = new(prerender: false);
    public static readonly InteractiveServerRenderMode ServerOnly = new(prerender: false);
}
