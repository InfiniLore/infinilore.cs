// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Kiota;
using InfiniLore.Kiota.Api.V1.Account.Profile.Item;
using InfiniLore.Kiota.Api.V1.Account.Profile.Item.ProfileImage;
using InfiniLore.Kiota.Models;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Modules.Core.Shared.Database;
using InfiniLore.Modules.Core.Shared.Extensions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;

namespace InfiniLore.Modules.Core.Wasm.Services.InteractiveApi;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IInteractiveApiUsers>]
public class InteractiveApiWasmUsers(
    ILogger<InteractiveApiWasmUsers> logger,
    IInteractiveApiWasm interactiveApi
) : IInteractiveApiUsers {
    private const int MaxFileSize = 5 * 1024 * 1024; // 5MB in bytes

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<Outcome<IInfiniLoreUserModel>> GetUserAsync(string userId, CancellationToken ct = default) {
        try {
            InfiniLoreApiClient client = interactiveApi.ApiClient;
            WithUserItemRequestBuilder requestBuilder = client.Api.V1.Account.Profile[userId];       
            KiotaUserUserProfileResponse? result = await requestBuilder.GetAsync(cancellationToken: ct);
            
            if (result is null) return Outcome<IInfiniLoreUserModel>.FromError(interactiveApi.DefaultApiError);

            IInfiniLoreUserModel model =  WasmInfiniLoreUserModel.FromKiotaModel(result);
            return Outcome.FromData(model);
        }

        catch (Exception e) {
            logger.Error(e, "Error getting user");
            return Outcome<IInfiniLoreUserModel>.FromError(interactiveApi.DefaultApiError);
        }
    }

    public async ValueTask<Outcome> UpsertProfileImageAsync(string userId, IBrowserFile file, CancellationToken ct = default) {
        try {
            InfiniLoreApiClient client = interactiveApi.ApiClient;
            ProfileImageRequestBuilder requestBuilder = client.Api.V1.Account.Profile[userId].ProfileImage;
        
            // Read the stream into a memory stream first
            await using MemoryStream stream = await file.ToMemoryStreamAsync(MaxFileSize, ct: ct);

            var multipartBody = new MultipartBody();
            multipartBody.AddOrReplacePart(
                "File",  // This must match exactly with the server-side model property name
                file.ContentType,
                stream,
                file.Name
            );
            await requestBuilder.PostAsync(multipartBody, cancellationToken: ct);
            return Outcome.FromState(true);
        }
        catch (Exception e) {
            logger.Error(e, "Error updating profile image");
            return Outcome.FromError(interactiveApi.DefaultApiError);       
        }
    }
}
