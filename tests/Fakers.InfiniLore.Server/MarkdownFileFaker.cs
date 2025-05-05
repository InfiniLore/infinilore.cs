// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Bogus;
using InfiniLore.Server.Modules.MarkdownFiles.Database;
using System.Collections.Concurrent;

namespace Fakers.InfiniLore.Server;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MarkdownFileFaker {
    private readonly ConcurrentDictionary<Guid, MarkdownFileModel> Entries = new();
    
    private static Faker<MarkdownFileModel> Faker { get; } = new Faker<MarkdownFileModel>()
        .RuleFor(property: x => x.Id, setter: f => f.Random.Guid())
        .RuleFor(property: x => x.OwnerId, setter: f => f.Random.Guid())
        .RuleFor(property: x => x.Name, setter: f => f.Random.AlphaNumeric(MarkdownFileModel.Defaults.NameMaxLength))
        .RuleFor(property: x => x.Source, setter: f => f.Lorem.Paragraphs(2)// Random string
        );
    
    private static MarkdownFileModel EntryWithFixedId(Guid fixedId, Guid loreScopeId) => new() {
        Id = fixedId,
        OwnerId = loreScopeId,
        Name = Faker.Generate().Name,
        Source = Faker.Generate().Source
    };

    public MarkdownFileModel GetById(Guid id, Guid loreScopeId) => Entries.GetOrAdd(
        id,
        valueFactory: static (guid, o) => EntryWithFixedId(guid, o),
        loreScopeId
    );
    
}
