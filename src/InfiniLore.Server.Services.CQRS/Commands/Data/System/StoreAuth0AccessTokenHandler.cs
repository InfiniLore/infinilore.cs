// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Data.System;
using InfiniLore.Server.Contracts.Services.Auth0;
using InfiniLore.Server.Database.Models.Data.System;
using MediatR;

namespace InfiniLore.Server.Services.CQRS.Commands.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class StoreAuth0AccessTokenHandler(IUnitOfWorkFactory unitOfWorkFactory, IAuth0AccessTokenEncryptionService encryptionService) : IRequestHandler<StoreAuth0AccessTokenRequest, MediatorResponse<bool>> {

    public async Task<MediatorResponse<bool>> Handle(StoreAuth0AccessTokenRequest request, CancellationToken ct) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var keyValueStoreRepository = await unitOfWork.GetRepositoryAsync<IKeyValueStoreRepository>(ct);

        // TODO encrypt the full body of Auth0AccessTokenJsonDto after it has been converted to a Jsom
        Auth0AccessTokenJsonDto token = Auth0AccessTokenJsonDto.FromToken(request.Token);
        var store = new KeyValueStore { Key = "Auth0AccessToken" };
        
        if (!store.CanSetObjectAsValueJson(token)) return MediatorResponse<bool>.FromFailureString("Cannot store auth0 access token. Json conversion failed.");
        if (!store.TrySetbOjectAsJsonValue(token)) return MediatorResponse<bool>.FromFailureString("Cannot store auth0 access token. Json conversion failed.");
        
        store.Value = encryptionService.Encrypt(store.Value);
        
        RepoResult result = await keyValueStoreRepository.TryAddOrUpdateAsync(store, ct);
        return result.IsSuccess;
    }
}
