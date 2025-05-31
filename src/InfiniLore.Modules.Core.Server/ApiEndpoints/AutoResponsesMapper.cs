// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Shared;

namespace InfiniLore.Modules.Core.Server.ApiEndpoints;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AutoResponsesMapper<TResponse, TResponseMapper, TResponses, TModel> : ResponseMapper<TResponses, PaginatedData<TModel>>
    where TModel : class 
    where TResponseMapper : ResponseMapper<TResponse, TModel>
    where TResponse : BasicResponse
    where TResponses : PaginatedResponse<TResponse>, new() 
{
    public override TResponses FromEntity(PaginatedData<TModel> entities) {
        var singleMapper = Resolve<TResponseMapper>();

        TResponse[] items = entities.Items.Select(singleMapper.FromEntity).ToArray();
        return new TResponses {
            Items = items,
            TotalCount = entities.TotalCount,
            TotalPages = entities.TotalPages,
            CurrentPage = entities.CurrentPage
        };
    }

    public override async Task<TResponses> FromEntityAsync(PaginatedData<TModel> entities, CancellationToken ct) {
        var singleMapper = Resolve<TResponseMapper>();

        IEnumerable<Task<TResponse>> items = entities.Items.Select(entity => singleMapper.FromEntityAsync(entity, ct));
        return new TResponses {
            Items = await Task.WhenAll(items),
            TotalCount = entities.TotalCount,
            TotalPages = entities.TotalPages,
            CurrentPage = entities.CurrentPage
        };
    }
}

