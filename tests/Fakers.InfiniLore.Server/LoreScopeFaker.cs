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
        .RuleFor(property: x => x.Name, setter: f => f.Random.AlphaNumeric(LoreScopeModel.Defaults.NameMaxLength))
        .RuleFor(property: x => x.ShortDescription, setter: f =>
                f.Random.Bool(0.2f)// 20% chance of empty description
                    ? string.Empty
                    : f.Lorem.Letter(LoreScopeModel.Defaults.ShortDescriptionMaxLength)// Random string
        );

    private static LoreScopeModel EntryWithFixedId(Guid fixedId, Guid ownerId) => new() {
        Id = fixedId,
        OwnerId = ownerId,
        Name = Faker.Generate().Name,
        ShortDescription = Faker.Generate().ShortDescription
    };

    public LoreScopeModel GetById(Guid id, Guid ownerId) => Entries.GetOrAdd(
        id,
        valueFactory: static (guid, o) => EntryWithFixedId(guid, o),
        ownerId
    );

    public static LoreScopeModel Generate() => Faker.Generate();
}
