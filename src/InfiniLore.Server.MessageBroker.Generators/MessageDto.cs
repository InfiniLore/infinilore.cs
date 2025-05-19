// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.GeneratorTools;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace InfiniLore.Server.MessageBroker.Generators;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MessageDto(INamedTypeSymbol symbol) {
    private IMethodSymbol? Constructor => symbol.InstanceConstructors
        .Where(c => !c.IsImplicitlyDeclared) // Filter out compiler-generated constructors, needed because records are special
        .FirstOrDefault(c => c.Parameters.Length > 0);
    
    public readonly bool IsEvent = symbol.AllInterfaces.Any(x => x.IsDisplayName(TypeNames.IEvent));
    
    public string ClassNameFull { get; } = symbol.ToDisplayString();
    
    public string MethodName => IsEvent
        ? $"Invoke{MethodNameRegex.Replace(symbol.Name, "Async")}"
        : MethodNameRegex.Replace(symbol.Name, "Async");
    
    private static readonly Regex MethodNameRegex = new(@"(?<=\w)(Query|Request|Command|Event|Async)", RegexOptions.Compiled);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ImmutableArray<string> GetArgDefinitions() {
        if (Constructor == null) return ImmutableArray<string>.Empty;
        
        return Constructor.Parameters
            .Select(p => $"{p.Type.ToDisplayString()} {GetArgName(p)}{GetDefaultValue(p)},")
            .ToImmutableArray();
    }

    public ImmutableArray<string> GetArgNames() {
        if (Constructor == null) return ImmutableArray<string>.Empty;
        
        return Constructor.Parameters
            .Select(GetArgName)
            .ToImmutableArray();
    }

    public string GetReturnType() {
        if (IsEvent) return "ValueTask";
        if (symbol.BaseType is null) return "ValueTask";

        string innerType = symbol.BaseType
            .Interfaces
            .FirstOrDefault(i => i.Name == "ICommand")?
            .TypeArguments[0]
            .ToDisplayString() ?? "UNDEFINED";
        return $"ValueTask<{innerType}>";
    } 
    
    private static string GetArgName(IParameterSymbol parameter) 
        => $"{char.ToLower(parameter.Name[0])}{parameter.Name.Substring(1)}";
    
    private static string GetDefaultValue(IParameterSymbol parameter) {
        if (!parameter.IsOptional) return "";
        if (parameter.ExplicitDefaultValue is not {} defaultValue) return " = default";
    
        return defaultValue switch {
            string s => $" = \"{s}\"",
            bool b => $" = {b.ToString().ToLowerInvariant()}",
            char c => $" = '{c}'",
            null => " = null",
            _ => $" = {defaultValue}"
        };
    }

}
