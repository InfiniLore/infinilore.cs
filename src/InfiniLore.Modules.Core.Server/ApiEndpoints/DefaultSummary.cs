// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;

namespace InfiniLore.Modules.Core.Server.ApiEndpoints;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class DefaultSummary : EndpointSummary{
    public DefaultSummary() {
        Responses[401] = "Unauthorized";
        // Responses[402] = "Payment Required";
        Responses[403] = "Forbidden";
        Responses[404] = "Not Found";
        Responses[500] = "Internal Server Error";
    }
}
