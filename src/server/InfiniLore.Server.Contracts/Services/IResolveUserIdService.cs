// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.API.Dto;
using InfiniLore.Server.Data.Models.Account;

namespace InfiniLore.Server.Contracts.Services;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IResolveUserIdService {
    public Task<InfiniLoreUser?> ResolveUserIdAsync<T>(T hasUserId, CancellationToken ct) where T : IRequiresUserId;
}
