// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace InfiniLore.Database.MsSqlServer.Configurations;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ConverterHelpers {
    public static readonly ValueConverter<ICollection<Guid>, string> GuidCollectionConverter =  new(
        v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
        v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions?)null) ?? new List<Guid>()
    );
    
    public static readonly ValueComparer<ICollection<Guid>> GuidCollectionComparer = new(
        (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
        c => c.Aggregate(0, (hash, guid) => HashCode.Combine(hash, guid.GetHashCode())),
        c => c.ToList()
    ); 
}
