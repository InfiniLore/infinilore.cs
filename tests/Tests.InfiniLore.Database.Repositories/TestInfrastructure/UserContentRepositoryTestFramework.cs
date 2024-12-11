// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using InfiniLore.Database.Models;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Contracts.Types;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using Tests.InfiniLore.Database.Repositories.TestInfrastructure;

namespace Tests.InfiniLore.Database.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class UserContentRepositoryTestFramework<TRepository, TModel>(DatabaseInfrastructure infrastructure)
    where TRepository : IUserContentRepository<TModel>
    where TModel : UserContent {
    private readonly TRepository _repository = infrastructure.ServiceProvider.GetRequiredService<TRepository>();

    private readonly IDbUnitOfWork<MsSqlDbContext> _unitOfWork =
        infrastructure.ServiceProvider.GetRequiredService<IDbUnitOfWork<MsSqlDbContext>>();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public abstract Task TestCanCreateSingleModel(TModel model);
    public abstract Task TestCanCreateMultipleModels(IEnumerable<TModel> models);

    public abstract Task TestCanUpdateModel(
        (TModel model, Func<TModel, ValueTask<TModel>> updateFunc, Func<TModel, bool> validateFunc) tuple
    );

    public abstract Task TestCanDeleteModel(TModel model);

    public abstract Task TestCanGetByIdAsync(TModel model);
    public abstract Task TestCanGetByUserAsync((UserIdUnion userUnion, TModel model) tuple);
    public abstract Task TestCanGetAllAsync(IEnumerable<TModel> models);
    public abstract Task TestCanGetByCriteriaAsync((Expression<Func<TModel, bool>> predicate, TModel model) tuple);

    #region Commands
    protected async Task CanCreateSingleModel(TModel model) {
        // Act
        RepoResult commandResult = await _repository.TryAddAsync(model);
        bool commitResult = await _unitOfWork.TryCommitAsync();

        // Assert
        await Assert.That(commitResult).IsTrue();
        await Assert.That(commandResult.IsSuccess).IsTrue();

        // Verify
        RepoResult<TModel> addedModel = await _repository.TryGetByIdAsync(model.Id);
        await Assert.That(addedModel.IsSuccess).IsTrue();
        await Assert.That(addedModel.TryGetSuccessValue(out TModel? addedModelValue)).IsTrue();
        await Assert.That(addedModelValue!.Id).IsEqualTo(model.Id);
    }

    protected async Task CanCreateMultipleModels(IEnumerable<TModel> models) {
        // Act
        IEnumerable<TModel> userContents = models as TModel[] ?? models.ToArray();
        RepoResult commandResult = await _repository.TryAddRangeAsync(userContents);
        bool commitResult = await _unitOfWork.TryCommitAsync();

        // Assert
        await Assert.That(commitResult).IsTrue();
        await Assert.That(commandResult.IsSuccess).IsTrue();

        // Verify
        foreach (TModel model in userContents) {
            RepoResult<TModel> addedModel = await _repository.TryGetByIdAsync(model.Id);
            await Assert.That(addedModel.IsSuccess).IsTrue();
            await Assert.That(addedModel.TryGetSuccessValue(out TModel? addedModelValue)).IsTrue();
            await Assert.That(addedModelValue!.Id).IsEqualTo(model.Id);
        }
    }

    protected async Task CanUpdateModel(
        (TModel model, Func<TModel, ValueTask<TModel>> updateFunc, Func<TModel, bool> validateFunc) tuple
    ) {
        // Arrange
        var (model, updateFunc, validateFunc) = tuple;
        await _repository.TryAddAsync(model);
        bool commitResult = await _unitOfWork.TryCommitAsync();
        model = await updateFunc(model);

        // Act
        RepoResult commandResult = await _repository.TryUpdateAsync(model);
        bool commitResult2 = await _unitOfWork.TryCommitAsync();

        // Assert
        await Assert.That(commitResult).IsTrue();
        await Assert.That(commitResult2).IsTrue();
        await Assert.That(commandResult.IsSuccess).IsTrue();

        // Verify
        RepoResult<TModel> updatedModel = await _repository.TryGetByIdAsync(model.Id);
        await Assert.That(updatedModel.IsSuccess).IsTrue();
        await Assert.That(updatedModel.TryGetSuccessValue(out TModel? value)).IsTrue();
        await Assert.That(validateFunc(value!)).IsTrue();
    }

    protected async Task CanDeleteModel(TModel model) {
        // Arrange
        await _repository.TryAddAsync(model);
        bool commitResult = await _unitOfWork.TryCommitAsync();

        // Act
        RepoResult commandResult = await _repository.TryDeleteAsync(model);
        bool commitResult2 = await _unitOfWork.TryCommitAsync();

        // Assert
        await Assert.That(commitResult).IsTrue();
        await Assert.That(commitResult2).IsTrue();
        await Assert.That(commandResult.IsSuccess).IsTrue();

        // Verify
        RepoResult<TModel> deletedModel = await _repository.TryGetByIdAsync(model.Id);
        await Assert.That(deletedModel.IsFailure).IsTrue();// should be deleted
        await Assert.That(deletedModel.AsFailure.Value).IsEqualTo("Content not found.");
    }
    #endregion

    #region Queries
    protected async Task CanGetByIdAsync(TModel model) {
        // Arrange
        await AddModelToDatabaseAsync(model);

        // Act
        RepoResult<TModel> result = await _repository.TryGetByIdAsync(model.Id);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.TryGetSuccessValue(out TModel? value)).IsTrue();
        await Assert.That(value!.Id).IsEqualTo(model.Id);
    }

    protected async Task CanGetByUserAsync((UserIdUnion userUnion, TModel model) tuple) {
        // Arrange
        var (userUnion, model) = tuple;
        await AddModelToDatabaseAsync(model);

        // Act
        RepoResult<TModel[]> result = await _repository.TryGetByUserAsync(userUnion);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.TryGetSuccessValue(out TModel[]? values)).IsTrue();
        await Assert.That(values).Contains(model);
        // m => m.Id == model.Id
    }

    protected async Task CanGetAllAsync(IEnumerable<TModel> models) {
        // Arrange
        RepoResult<TModel[]> originalAmountResult = await _repository.TryGetAllAsync();
        await Assert.That(originalAmountResult.TryGetSuccessValue(out TModel[]? originalModels)).IsTrue();
        int originalAmount =
            originalModels!.Length;// We need to do this because we are using the same database for all tests

        IEnumerable<TModel> userContents = models as TModel[] ?? models.ToArray();
        foreach (TModel model in userContents) {
            await AddModelToDatabaseAsync(model);
        }

        // Act
        RepoResult<TModel[]> result = await _repository.TryGetAllAsync();

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.TryGetSuccessValue(out TModel[]? values)).IsTrue();
        await Assert.That(values!.Length).IsEqualTo(userContents.Count() + originalAmount);
    }

    protected async Task CanGetByCriteriaAsync((Expression<Func<TModel, bool>> predicate, TModel model) tuple) {
        // Arrange
        var (predicate, model) = tuple;
        await AddModelToDatabaseAsync(model);

        // Act
        RepoResult<TModel[]> result = await _repository.TryGetByCriteriaAsync(predicate);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.TryGetSuccessValue(out TModel[]? values)).IsTrue();
        await Assert.That(values).Contains(model);
    }

    private async Task AddModelToDatabaseAsync(TModel model) {
        var repository = infrastructure.ServiceProvider.GetRequiredService<TRepository>();
        await repository.TryAddAsync(model);
        await _unitOfWork.TryCommitAsync();
    }
    #endregion
}
