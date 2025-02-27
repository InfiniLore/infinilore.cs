// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace Tests.InfiniLore.Server.Database.Fakers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GuidStore {
    private static readonly ConcurrentDictionary<int, Guid> Guids = new();
    
    public static Guid GetGuid(int? seed = null) {
        // ReSharper disable once InvertIf
        if (seed is null) {
            seed = Random.Shared.Next();
            while (Guids.ContainsKey(seed.Value)) {
                seed = Random.Shared.Next();
            }
        }
        
        return Guids.GetOrAdd(seed.Value, i => {
            byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(i.ToString()));
            return new Guid(hash.Take(16).ToArray());
        });
    }
}
