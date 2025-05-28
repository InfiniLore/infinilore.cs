// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Database;
using InfiniLore.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.Core.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class S3FileRepository(ILogger<S3FileRepository> logger, IS3FileStorageService fileStorageService) : UnitOfWorkRepository<ContentDb>, IS3FileRepository{

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected virtual IQueryable<S3FileMetaDataModel> OptionalInclude(IQueryable<S3FileMetaDataModel> query) => query;
    protected virtual IQueryable<S3FileMetaDataModel> AlwaysInclude(IQueryable<S3FileMetaDataModel> query) => query;

    protected IQueryable<S3FileMetaDataModel> GetConfiguredQueryable(IQueryable<S3FileMetaDataModel> baseQuery, QueryConfig config) {
        IQueryable<S3FileMetaDataModel> query = baseQuery
            .With(AlwaysInclude)
            .ConditionalWith(config.OptionalInclude, OptionalInclude)
            .ConditionalReverse(config.Reverse)
            .ConditionalWith(config.RetrieveSoftDeleted, model => model.IgnoreQueryFilters());

        return query;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<Result> AddAsync(S3FileMetaDataModel model, CancellationToken ct = default) => throw new NotImplementedException();
    public async ValueTask<Result> AddRangeAsync(IEnumerable<S3FileMetaDataModel> models, CancellationToken ct = default) => throw new NotImplementedException();
    public async ValueTask<Result> UpdateAsync(S3FileMetaDataModel model, CancellationToken ct = default) => throw new NotImplementedException();
    public async ValueTask<Result> UpdateRangeAsync(IEnumerable<S3FileMetaDataModel> models, CancellationToken ct = default) => throw new NotImplementedException();
    public async ValueTask<Result> AddOrUpdateAsync(S3FileMetaDataModel model, CancellationToken ct = default) => throw new NotImplementedException();
    public async ValueTask<Result> AddOrUpdateRangeAsync(IEnumerable<S3FileMetaDataModel> models, CancellationToken ct = default) => throw new NotImplementedException();
    public async ValueTask<Result> DeleteAsync(S3FileMetaDataModel model, CancellationToken ct = default) => throw new NotImplementedException();
    public async ValueTask<Result> DeleteByIdAsync(Guid id, CancellationToken ct = default) => throw new NotImplementedException();
    public async ValueTask<Result> DeleteRangeAsync(IEnumerable<S3FileMetaDataModel> models, CancellationToken ct = default) => throw new NotImplementedException();
    public async ValueTask<Result> DeleteRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default) => throw new NotImplementedException();
    public async ValueTask<Result> RemoveAsync(S3FileMetaDataModel model, CancellationToken ct = default) => throw new NotImplementedException();
    public async ValueTask<Result> RemoveByIdAsync(Guid id, CancellationToken ct = default) => throw new NotImplementedException();
    public async ValueTask<Result> RemoveRangeAsync(IEnumerable<S3FileMetaDataModel> models, CancellationToken ct = default) => throw new NotImplementedException();
    public async ValueTask<Result> RemoveRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default) => throw new NotImplementedException();
    public async ValueTask<Result<S3FileMetaDataModel>> GetByIdAsync(Guid id, QueryConfig config = default, CancellationToken ct = default) => throw new NotImplementedException();
    public async ValueTask<Result<S3FileMetaDataModel[]>> GetAllAsync(QueryConfig config = default, CancellationToken ct = default) => throw new NotImplementedException();
    public async ValueTask<PaginatedResult<S3FileMetaDataModel>> GetAllAsync(PaginationInfo pageInfo, QueryConfig config = default, CancellationToken ct = default) => throw new NotImplementedException();
    public async ValueTask<Result<int>> GetCountAsync(CancellationToken ct = default) => throw new NotImplementedException();
    public async ValueTask<Result> IsIdTakenAsync(Guid id, CancellationToken ct = default) => throw new NotImplementedException();
    public async ValueTask<Result> IsIdNotTakenAsync(Guid id, CancellationToken ct = default) => throw new NotImplementedException();
    public async ValueTask<Result> TryIntializeBucketAsync(CancellationToken ct = default) => throw new NotImplementedException();
}
