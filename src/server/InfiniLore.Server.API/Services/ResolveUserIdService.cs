// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.API.Contracts;
using InfiniLore.Server.Database;
using InfiniLore.Server.Database.Models.Account;
using Microsoft.AspNetCore.Http;

namespace InfiniLore.Server.API.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[RegisterService<ResolveUserIdService>(LifeTime.Scoped)]
public class ResolveUserIdService(HttpContext httpContext) {
    
    public async Task<AsyncResult<InfiniLoreUser>> ResolveUserIdAsync<T>(InfiniLoreDbContext dbContext, T hasUserId, CancellationToken ct) where T : IRequiresUserId {
        if (hasUserId.UserId == Guid.Empty) {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") {
                return AsyncResult<InfiniLoreUser>.Failure(new ProblemDetails {
                    Status = StatusCodes.Status404NotFound,
                    Detail = "The userid was not provided.",
                    Instance = httpContext.Request.Path.ToString(),
                });
            }
            
        }
        if (await dbContext.Users.FindAsync([hasUserId.UserId.ToString()], ct) is not {} user) 
            return AsyncResult<InfiniLoreUser>.Failure(new ProblemDetails {
                Status = StatusCodes.Status404NotFound,
                Detail = "The user with the given id was not found.",
                Instance = httpContext.Request.Path.ToString()
            });
        return user;
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
