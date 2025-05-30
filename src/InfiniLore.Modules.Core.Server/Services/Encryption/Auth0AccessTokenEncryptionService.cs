// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace InfiniLore.Modules.Core.Server.Encryption;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<IAuth0AccessTokenEncryptionService>]
public class Auth0AccessTokenEncryptionService(IOptions<Auth0AccessTokenEncryptionServiceOptions> options, ILogger<Auth0AccessTokenEncryptionService> logger) : IAuth0AccessTokenEncryptionService {

    private byte[] _ivCache = [];
    private byte[] Key { get; } = GetKey(options.Value, logger);
    private byte[] Iv => _ivCache.Length == 0 ? _ivCache = Key[..16] : _ivCache;

    public string Encrypt(string plainText) {
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = Iv;

        using ICryptoTransform encryptor = aes.CreateEncryptor();
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] cipherTextBytes = encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);

        return Convert.ToBase64String(cipherTextBytes);
    }

    public string Decrypt(string cipherText) {
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = Iv;

        using ICryptoTransform decryptor = aes.CreateDecryptor();
        byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
        byte[] plainTextBytes = decryptor.TransformFinalBlock(cipherTextBytes, 0, cipherTextBytes.Length);

        return Encoding.UTF8.GetString(plainTextBytes);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static byte[] GetKey(Auth0AccessTokenEncryptionServiceOptions options, ILogger logger) {
        if (options.SecretKey == Auth0AccessTokenEncryptionServiceOptions.DefaultSecretKey) {
            logger.Warning("Using default secret key for Auth0 access token encryption. This is not recommended for production.");
        }

        return SHA256.HashData(Encoding.UTF8.GetBytes(options.SecretKey));
    }
}
