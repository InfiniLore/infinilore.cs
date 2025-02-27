// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace DataSources.InfiniLore.Server;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GuidStore {
    private readonly ConcurrentDictionary<int, Guid> Guids = new();
    
    public Guid GetGuid(string seed) =>  Guids.GetOrAdd(seed.GetHashCode(), ValueFactory);
    public Guid GetGuid(int? seed = null) {
        if (seed is not null) return Guids.GetOrAdd(seed.Value, ValueFactory);
        
        seed = Random.Shared.Next();
        while (Guids.ContainsKey(seed.Value)) {
            seed = Random.Shared.Next();
        }

        return Guids.GetOrAdd(seed.Value, ValueFactory);
    }
    
    private static Guid ValueFactory(int i) {
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(i.ToString()));
        return new Guid(hash.AsSpan(0, 16));
    }
}
