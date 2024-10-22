// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Data;
using InfiniLore.Server.Data.Models.Account;
using InfiniLore.Server.Data.Models.Base;
using InfiniLoreLib.Results;

namespace InfiniLore.Server.Data.Repositories.Command.Base;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class UserContentRepository<T>(IDbUnitOfWork<InfiniLoreDbContext> unitOfWork) where T : UserContent<T> {
    public async Task<BoolResult> AddUserAccessAsync(T content, InfiniLoreUser user, AccessLevel accessLevel) {
        InfiniLoreDbContext dbContext = unitOfWork.GetDbContext();
        
        content.UserAccess.Add(new UserContentAccess<T> {
            User = user,
            AccessLevel = accessLevel
        });

        dbContext.Set<T>().Update(content);
        return BoolResult.Success();
    }
}
