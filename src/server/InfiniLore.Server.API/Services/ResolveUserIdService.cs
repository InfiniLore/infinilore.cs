// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.API.Dto;
using InfiniLore.Server.Contracts.Data;
using InfiniLore.Server.Contracts.Services;
using InfiniLore.Server.Data;
using InfiniLore.Server.Data.Models.Account;
using InfiniLore.Server.Data.Models.Base;
using Microsoft.AspNetCore.Http;

namespace InfiniLore.Server.API.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[RegisterService<IResolveUserIdService>(LifeTime.Scoped)]
public class ResolveUserIdService(IDbUnitOfWork<InfiniLoreDbContext> unitOfWork, HttpContext httpContext) : IResolveUserIdService {
    private readonly InfiniLoreDbContext _dbContext = unitOfWork.GetDbContext();
    
    public async Task<InfiniLoreUser?> ResolveUserIdAsync<T>(T hasUserId, CancellationToken ct) where T : IRequiresUserId {
        return await _dbContext.Users.FindAsync([hasUserId.UserId], ct);
    }
}

public class AsyncResult<T> {
    public T? Value { get; private init; }
    public bool Success { get; private init; }
    public IResult? FailedIResult { get; private init; }
    
    public static implicit operator AsyncResult<T>(bool success) => new() { Success = success };
    public static implicit operator AsyncResult<T>(T value) => new() { Success = true, Value = value };
    public static implicit operator bool (AsyncResult<T> result) => result.Success;

    public static AsyncResult<T> Failure<TResult>(TResult failedIResult) where TResult : IResult => new() { Success = false, FailedIResult = failedIResult };
}
