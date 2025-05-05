// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Database.Models;
using InfiniLore.Server.Modules.Core.Messaging;

namespace InfiniLore.Server.Modules.Users.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record GetUserByAuth0IdQuery(
    string Auth0Id
) : MessageRequest<IInfiniLoreUser>;
