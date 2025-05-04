// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.Users.Database;

namespace InfiniLore.Server.Modules.Users.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record GetUserByAuth0IdQuery(
    string Auth0Id
) : MessageRequest<IInfiniLoreUser>;
