using LingoMqCryptography.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace LingoMqCryptography.Cryptors;

public class Sha256Alghoritm : ICryptAlghoritm
{
    public BaseKeyPair Crypt(string word)
    {
        byte[] salt = CreateSalt(128 / 8);
        var hash = KeyDerivation.Pbkdf2(
            password: word, 
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8);

        return new Sha256KeyPair() {
            Hash = Convert.ToBase64String(hash),
            Salt = Convert.ToBase64String(salt)
        };
    }
    public bool Validate(string word, BaseKeyPair validKeys)
    {
        var hash = KeyDerivation.Pbkdf2(
            password: word,
            salt: Convert.FromBase64String(validKeys.Salt!),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8);

        return string.Equals(Convert.ToBase64String(hash),
            validKeys.Hash,
            StringComparison.OrdinalIgnoreCase);
    }
    private byte[] CreateSalt(int size)
    {
        byte[] buffer = RandomNumberGenerator.GetBytes(size);
        return buffer;
    }
}