// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Bogus;
using InfiniLore.Server.Modules.LoreScopes.Database;
using System.Collections.Concurrent;

namespace Fakers.InfiniLore.Server;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoreScopeFaker {
    private readonly ConcurrentDictionary<Guid, LoreScopeModel> Entries = new();

    private static Faker<LoreScopeModel> Faker { get; } = new Faker<LoreScopeModel>()
        .RuleFor(property: x => x.Id, setter: f => f.Random.Guid())
        .RuleFor(property: x => x.OwnerId, setter: f => f.Random.Guid())
        .RuleFor(property: x => x.Name, setter: f => f.Random.AlphaNumeric(LoreScopeModel.Defaults.NameMaxLength));

    private static LoreScopeModel EntryWithFixedId(Guid fixedId, Guid ownerId) => new() {
        Id = fixedId,
        OwnerId = ownerId,
        Name = Faker.Generate().Name,
        Description = null
    };

    public LoreScopeModel GetById(Guid id, Guid ownerId) => Entries.GetOrAdd(
        id,
        valueFactory: static (guid, o) => EntryWithFixedId(guid, o),
        ownerId
    );

    public static LoreScopeModel Generate() => Faker.Generate();
}
