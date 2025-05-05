// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Bogus;
using InfiniLore.Server.Modules.Users.Database;
using System.Collections.Concurrent;

namespace Fakers.InfiniLore.Server;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniLoreUserFaker {
    private static readonly ConcurrentBag<string> UsedUsernames = [];
    private static readonly ConcurrentBag<string> UsedAuth0MailPasswords = [];
    private readonly ConcurrentDictionary<Guid, InfiniLoreUserModel> Entries = new();

    private static Faker<InfiniLoreUserModel> Faker { get; } = new Faker<InfiniLoreUserModel>()
        .RuleFor(property: x => x.Id, setter: f => f.Random.Guid())
        .RuleFor(property: x => x.Username, GenerateUniqueUsername)
        .RuleFor(property: x => x.Auth0MailPassword, GenerateUniqueAuth0MailPassword);

    private static string GenerateUniqueUsername(Faker f) {
        string username;
        do {
            username = f.Internet.UserName();
        } while (UsedUsernames.Contains(username));

        UsedUsernames.Add(username);
        return username;
    }

    private static string GenerateUniqueAuth0MailPassword(Faker f) {
        string auth0MailPassword;
        do {
            auth0MailPassword = $"auth0|{f.Random.Replace("??????????")}";
        } while (UsedAuth0MailPasswords.Contains(auth0MailPassword));

        UsedAuth0MailPasswords.Add(auth0MailPassword);
        return auth0MailPassword;
    }


    private static InfiniLoreUserModel EntryWithFixedId(Guid fixedId) => new() {
        Id = fixedId,
        Username = Faker.Generate().Username,
        Auth0MailPassword = Faker.Generate().Auth0MailPassword
    };

    public InfiniLoreUserModel GetById(Guid id) => Entries.GetOrAdd(
        id,
        EntryWithFixedId
    );
}
