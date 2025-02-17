// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Bogus;
using InfiniLore.Server.Database.Models.Data.System;

namespace Tests.InfiniLore.Server.Database.Stores;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class KeyValueStoreFaker {
    public static Faker<KeyValueStore> Faker { get; } = new Faker<KeyValueStore>()
        .RuleFor(x => x.Id, f => f.Random.Guid())
        .RuleFor(x => x.Key, f => f.Random.String(10))
        .RuleFor(x => x.Value, f => f.Random.String(10));
    
    public static KeyValueStore EntryWithFixedId(Guid fixedId) => Faker
        .CustomInstantiator(f => new KeyValueStore {
            Id = fixedId,
            Key = f.Random.String2(10, 30), 
            Value = f.Random.String2(10, 30) 
        }).Generate();


    public static Guid Entry001Id = "f9176b5f-a3d3-4d6b-9fab-07b5e1ec950a".ToGuid();
    public static KeyValueStore Entry001 = EntryWithFixedId(Entry001Id);
}
