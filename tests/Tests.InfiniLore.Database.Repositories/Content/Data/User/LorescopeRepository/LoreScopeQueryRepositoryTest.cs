// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using InfiniLore.Database.Models.Content.UserData;
using InfiniLore.Server.Contracts.Database.Repositories.Content.Data.User;
using InfiniLore.Server.Contracts.Types;
using System.Linq.Expressions;
using Tests.InfiniLore.Database.Repositories.Data;
using Tests.InfiniLore.Database.Repositories.TestInfrastructure;

namespace Tests.InfiniLore.Database.Repositories.Content.Data.User.LorescopeRepository;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[ClassDataSource<DatabaseInfrastructure>(Shared = SharedType.PerTestSession)]
[NotInParallel]
public class LoreScopeRepositoryTest(DatabaseInfrastructure infrastructure)
    : UserContentRepositoryTestFramework<ILorescopeRepository, LorescopeModel>(infrastructure) {

    [Test]
    [MethodDataSource(typeof(LoreScopeCommandTestData), nameof(LoreScopeCommandTestData.GetSingleModels))]
    public override Task TestCanCreateSingleModel(LorescopeModel model)
        => CanCreateSingleModel(model);

    [Test]
    [MethodDataSource(typeof(LoreScopeCommandTestData), nameof(LoreScopeCommandTestData.GetMultipleModels))]
    public override Task TestCanCreateMultipleModels(IEnumerable<LorescopeModel> models)
        => CanCreateMultipleModels(models);

    [Test]
    [MethodDataSource(typeof(LoreScopeCommandTestData), nameof(LoreScopeCommandTestData.GetUpdate))]
    public override Task TestCanUpdateModel(
        (LorescopeModel model, Func<LorescopeModel, ValueTask<LorescopeModel>> updateFunc, Func<LorescopeModel, bool>
            validateFunc) tuple
    )
        => CanUpdateModel(tuple);

    [Test]
    [MethodDataSource(typeof(LoreScopeCommandTestData), nameof(LoreScopeCommandTestData.GetDeletes))]
    public override Task TestCanDeleteModel(LorescopeModel model)
        => CanDeleteModel(model);

    [Test]
    [MethodDataSource(typeof(LoreScopeCommandTestData), nameof(LoreScopeQueryTestData.GetSingleModels))]
    public override Task TestCanGetByIdAsync(LorescopeModel model)
        => CanGetByIdAsync(model);

    [Test]
    [MethodDataSource(typeof(LoreScopeQueryTestData), nameof(LoreScopeQueryTestData.GetUserModels))]
    public override Task TestCanGetByUserAsync((UserIdUnion userUnion, LorescopeModel model) tuple)
        => CanGetByUserAsync(tuple);

    [Test]
    [MethodDataSource(typeof(LoreScopeCommandTestData), nameof(LoreScopeQueryTestData.GetMultipleModels))]
    public override Task TestCanGetAllAsync(IEnumerable<LorescopeModel> models)
        => CanGetAllAsync(models);

    [Test]
    [MethodDataSource(typeof(LoreScopeQueryTestData), nameof(LoreScopeQueryTestData.GetCriteriaModels))]
    public override Task TestCanGetByCriteriaAsync(
        (Expression<Func<LorescopeModel, bool>> predicate, LorescopeModel model) tuple
    )
        => CanGetByCriteriaAsync(tuple);
}
