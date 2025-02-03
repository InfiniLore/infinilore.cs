// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;

namespace Tools.InfiniLore.Commands.ApiToken;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly partial struct ApiTokenParameters : IParameters {
    [CliArgsParameter("user", "u")] [CliArgsDescription("The user's Username")]
    public required string User { get; init; }

    [CliArgsParameter("password", "p")] [CliArgsDescription("The user's Password")]
    public required string Password { get; init; }

    [CliArgsParameter("copy", "c")] [CliArgsDescription("Copy the token to the clipboard")]
    public string? CopyValue { get; init; } = null;

    public SectionToCopy Copy => Enum.Parse<SectionToCopy>(CopyValue ?? "None", true);
}

public enum SectionToCopy {
    None,
    ApiToken,
    JwtRefreshToken
}
