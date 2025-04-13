// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace InfiniLore.Server.Database.Models.Data.System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// A key-value store entry which contains a <see langword="string"/> an associated unique <see cref="Key"/>.
/// </summary>
public class KeyValueStore {
    /// <summary>
    /// A unique <c>string</c> associated with a <c>Value</c>
    /// </summary>
    [MaxLength(Defaults.KeyMaxLength)] public required string Key { get; set; } = string.Empty;

    /// <summary>
    /// a <c>string</c> associated with a unique <c>Key</c>.
    /// </summary>
    [MaxLength(Defaults.ValueMaxLength)] public string? Value { get; set; }

    /// <summary>
    /// Decodes <see cref="Value"/> into an instance of <typeparamref name="TJsonObject"/> which it writes into <paramref name="decodedObject"/>.
    /// </summary>
    /// <param name="decodedObject">instance of <typeparamref name="TJsonObject"/> that has been decoded, otherwise <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the decoding was successful,
    /// <see langword="false"/> otherwise.
    /// </returns>
    public bool TryGetConvertJsonValueToObject<TJsonObject>([NotNullWhen(true)] out TJsonObject? decodedObject) where TJsonObject : class {
        decodedObject = null;
        if (Value.IsNullOrWhiteSpace()) return false;

        // Try deserializing Value to the specified type TJsonObject
        try {
            decodedObject = JsonSerializer.Deserialize<TJsonObject>(Value);
            return decodedObject != null;
        }
        catch (JsonException) {
            return false;
        }
    }
    /// <summary>
    /// Encodes <paramref name="objectToEncode"/> as a JSON encoded <see langword="string"/> which it writes into <see cref="Value"/>.
    /// </summary>
    /// <param name="objectToEncode">instance of <typeparamref name="TJsonObject"/> to encode.</param>
    /// <returns>
    /// <see langword="true"/> if the encoding was successful,
    /// <see langword="false"/> otherwise.
    /// </returns>
    [MemberNotNullWhen(true, nameof(Value))]
    public bool TrySetObjectAsJsonValue<TJsonObject>(in TJsonObject objectToEncode) where TJsonObject : class {
        try {
            string json = JsonSerializer.Serialize(objectToEncode);
            if (json.Length > Defaults.ValueMaxLength) return false;

            Value = json;
            return true;
        }
        catch (JsonException) {
            return false;
        }
    }
    /// <summary>
    /// Checks if the <paramref name="objectToEncode"/> instance of <typeparamref name="TJsonObject"/> can be encoded as JSON.
    /// </summary>
    /// <param name="objectToEncode">The instance of <typeparamref name="TJsonObject"/> to check if it can be encoded for</param>
    /// <returns><see langword="true"/> if the <paramref name="objectToEncode"/> can be encoded, otherwise <see langword="false"/></returns>
    public bool CanSetObjectAsValueJson<TJsonObject>(in TJsonObject objectToEncode) where TJsonObject : class {
        try {
            string json = JsonSerializer.Serialize(objectToEncode);
            return json.Length <= Defaults.ValueMaxLength;
        }
        catch (Exception ex) {
            Console.WriteLine(ex);
            return false;
        }
    }
    /// <summary>
    /// Default limits for <see cref="KeyValueStore"/>.
    /// </summary>
    public static class Defaults {
        /// <summary>
        /// The max length any <see cref="Key"/> can have.
        /// </summary>
        public const int KeyMaxLength = 256;
        /// <summary>
        /// The max length any <see cref="Value"/> can have.
        /// </summary>
        public const int ValueMaxLength = int.MaxValue - 1;
    }
}
