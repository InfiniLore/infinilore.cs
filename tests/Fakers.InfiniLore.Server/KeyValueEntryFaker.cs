// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Bogus;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Modules.Core.Server.Database;
using JetBrains.Annotations;
using System.Collections.Concurrent;

namespace Fakers.InfiniLore.Server;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<KeyValueEntryFaker>]
public class KeyValueEntryFaker {
    [UsedImplicitly] private readonly ConcurrentDictionary<Guid, KeyValueEntryModel> Entries = new();

    public Faker<KeyValueEntryModel> Faker { get; } = new Faker<KeyValueEntryModel>()
        .RuleFor(property: x => x.Key, setter: f => f.Random.AlphaNumeric(10))
        .RuleFor(property: x => x.Value, setter: f => f.Random.AlphaNumeric(10));
}
