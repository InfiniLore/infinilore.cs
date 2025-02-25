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
    public Faker<KeyValueStore> Faker { get; } = new Faker<KeyValueStore>()
        .RuleFor(x => x.Id, f => f.Random.Guid())
        .RuleFor(x => x.Key, f => f.Random.AlphaNumeric(10))
        .RuleFor(x => x.Value, f => f.Random.AlphaNumeric(10));
    
    public KeyValueStore EntryWithFixedId(Guid fixedId) => new() {
        Id = fixedId,
        Key = Faker.Generate().Key,
        Value = Faker.Generate().Value
    };

    private readonly ConcurrentDictionary<Guid, KeyValueStore> Entries = new();
    
    public KeyValueStore GetById(Guid id) => Entries.GetOrAdd(id, EntryWithFixedId);
    public KeyValueStore GetById(GuidStore id) => Entries.GetOrAdd(id.ToGuid(), EntryWithFixedId);
        
}
