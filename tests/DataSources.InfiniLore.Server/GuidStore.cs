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
    private readonly ConcurrentDictionary<string, Guid> StringGuids = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public Guid GetGuid(string seed) => StringGuids.GetOrAdd(seed, ValueFactory);
    public Guid GetGuid(int? seed = null) {
        if (seed is not null) return Guids.GetOrAdd(seed.Value, ValueFactory);

        seed = Random.Shared.Next();
        while (Guids.ContainsKey(seed.Value)) {
            seed = Random.Shared.Next();
        }

        return Guids.GetOrAdd(seed.Value, ValueFactory);
    }

    private static Guid ValueFactory(int i) => ValueFactory(i.ToString());

    private static Guid ValueFactory(string i) {
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(i));
        return new Guid(hash.AsSpan(0, 16));
    }
}
