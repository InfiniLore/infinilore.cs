// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Bogus;
using InfiniLore.Server.Database.Models.Data.System;
using System.Collections.Concurrent;

namespace Tests.InfiniLore.Server.Database.Fakers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class KeyValueStoreFaker {
    private readonly ConcurrentDictionary<Guid, KeyValueStore> Entries = new();

    public Faker<KeyValueStore> Faker { get; } = new Faker<KeyValueStore>()
        .RuleFor(property: x => x.Id, setter: f => f.Random.Guid())
        .RuleFor(property: x => x.Key, setter: f => f.Random.AlphaNumeric(10))
        .RuleFor(property: x => x.Value, setter: f => f.Random.AlphaNumeric(10));

    public KeyValueStore EntryWithFixedId(Guid fixedId) => new() {
        Id = fixedId,
        Key = Faker.Generate().Key,
        Value = Faker.Generate().Value
    };

    public KeyValueStore GetById(Guid id) => Entries.GetOrAdd(id, EntryWithFixedId);
}
