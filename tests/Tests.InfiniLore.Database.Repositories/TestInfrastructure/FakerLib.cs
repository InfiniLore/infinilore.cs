// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Bogus;
using InfiniLore.Database.Models.Content.Data.System;

namespace Tests.InfiniLore.Database.Repositories.TestInfrastructure;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class FakerLib {
    public static readonly Faker<InfiniLorePermission> InfiniLorePermission = new Faker<InfiniLorePermission>()
        .RuleFor(property: p => p.Id, setter: _ => Guid.NewGuid())// Just use a random guid for the id, instead of the version7 guid.
        .RuleFor(property: p => p.Name, setter: _ => Guid.NewGuid().ToString().Truncate(255)) // To ensure they are unique
        .RuleFor(property: p => p.Description, setter: f => f.Lorem.Sentence().Truncate(511));
}
