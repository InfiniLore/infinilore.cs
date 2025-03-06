// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Bogus;
using InfiniLore.Server.Database.Models.Data.System;
using System.Collections.Concurrent;

namespace Fakers.InfiniLore.Server;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class KeyValueStoreFaker {
    private readonly ConcurrentDictionary<Guid, KeyValueStore> Entries = new();

    public Faker<KeyValueStore> Faker { get; } = new Faker<KeyValueStore>()
        .RuleFor(property: x => x.Key, setter: f => f.Random.AlphaNumeric(10))
        .RuleFor(property: x => x.Value, setter: f => f.Random.AlphaNumeric(10));
}
