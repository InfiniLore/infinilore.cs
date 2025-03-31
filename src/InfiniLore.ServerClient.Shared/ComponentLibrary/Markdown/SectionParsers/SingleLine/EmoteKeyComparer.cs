// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.SectionParsers.SingleLine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EmoteKeyComparer :  IEqualityComparer<EmoteKey>, IAlternateEqualityComparer<string, EmoteKey> {
    public bool Equals(EmoteKey? x, EmoteKey? y)
        => x is not null && y is not null && x.Equals(y) ;
    
    public int GetHashCode(EmoteKey obj) 
        => obj.GetHashCode();
    
    public bool Equals(string alternate, EmoteKey other) 
        => other.Keys.Contains(alternate);
    
    public int GetHashCode(string alternate)
        => alternate.GetHashCode();
    
    public EmoteKey Create(string alternate) => throw new NotSupportedException("Cannot create registration based on partial key");
}
