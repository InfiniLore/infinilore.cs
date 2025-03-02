// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Bogus;
using InfiniLore.Server.Database.Models.Data.User;
using System.Collections.Concurrent;

namespace Fakers.InfiniLore.Server;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoreScopeFaker {
    private readonly ConcurrentDictionary<Guid, LoreScope> Entries = new();

    private static Faker<LoreScope> Faker { get; } = new Faker<LoreScope>()
        .RuleFor(property: x => x.Id, setter: f => f.Random.Guid())
        .RuleFor(property: x => x.OwnerId, setter: f => f.Random.Guid())
        .RuleFor(property: x => x.Name, setter: f => f.Random.AlphaNumeric(LoreScope.Defaults.NameMaxLength))
        .RuleFor(property: x => x.ShortDescription, setter: f => 
                f.Random.Bool(0.2f) // 20% chance of empty description
                    ? string.Empty
                    : f.Lorem.Letter(LoreScope.Defaults.ShortDescriptionMaxLength) // Random string
        );

    private static LoreScope EntryWithFixedId(Guid fixedId, Guid ownerId) => new() {
        Id = fixedId,
        OwnerId = ownerId,
        Name = Faker.Generate().Name,
        ShortDescription = Faker.Generate().ShortDescription,
    };

    public LoreScope GetById(Guid id, Guid ownerId) => Entries.GetOrAdd(
        id,
        static (guid, o) => EntryWithFixedId(guid, o),
        ownerId
    );

    public static LoreScope Generate() => Faker.Generate();
}
