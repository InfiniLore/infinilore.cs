// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Bogus;
using InfiniLore.Server.Database.Models.Data.Project;
using InfiniLore.Server.Database.Models.Data.User;
using System.Collections.Concurrent;

namespace Fakers.InfiniLore.Server;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MarkdownFileFaker {
    private readonly ConcurrentDictionary<Guid, MarkdownFile> Entries = new();
    
    private static Faker<MarkdownFile> Faker { get; } = new Faker<MarkdownFile>()
        .RuleFor(property: x => x.Id, setter: f => f.Random.Guid())
        .RuleFor(property: x => x.LoreScopeId, setter: f => f.Random.Guid())
        .RuleFor(property: x => x.Name, setter: f => f.Random.AlphaNumeric(MarkdownFile.Defaults.NameMaxLength))
        .RuleFor(property: x => x.Source, setter: f => f.Lorem.Paragraphs(2)// Random string
        );
    
    private static MarkdownFile EntryWithFixedId(Guid fixedId, Guid loreScopeId) => new() {
        Id = fixedId,
        LoreScopeId = loreScopeId,
        Name = Faker.Generate().Name,
        Source = Faker.Generate().Source
    };

    public MarkdownFile GetById(Guid id, Guid loreScopeId) => Entries.GetOrAdd(
        id,
        valueFactory: static (guid, o) => EntryWithFixedId(guid, o),
        loreScopeId
    );
    
}
