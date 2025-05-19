// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading;
using CodeOfChaos.GeneratorTools;
using System.Collections.Immutable;

namespace InfiniLore.Server.MessageBroker.Generators;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Generator(LanguageNames.CSharp)]
public class MessageBrokerGenerator : IIncrementalGenerator {

    public void Initialize(IncrementalGeneratorInitializationContext context) {
        IncrementalValueProvider<ImmutableArray<MessageDto?>> data = context.SyntaxProvider
            .CreateSyntaxProvider(Predicate, Transform)
            .Where(static n => n is not null)
            .Collect();
    }
    
    private static bool Predicate(SyntaxNode node, CancellationToken _) 
        => node is ClassDeclarationSyntax or RecordDeclarationSyntax;

    private static MessageDto? Transform(GeneratorSyntaxContext ctx, CancellationToken _) {
        SemanticModel semanticModel = ctx.SemanticModel;
        
        INamedTypeSymbol? symbol = ctx.Node switch {
            ClassDeclarationSyntax classDeclaration => semanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol,
            RecordDeclarationSyntax recordDeclaration => semanticModel.GetDeclaredSymbol(recordDeclaration) as INamedTypeSymbol,
            _ => null
        };
        
        if (symbol is null) return null;
        if (!symbol.HasInterfaceWithDisplayName(TypeNames.ICommandBase) 
            || !symbol.HasInterfaceWithDisplayName(TypeNames.IEvent)) return null!;
        
        return new MessageDto(symbol);
    }
}
