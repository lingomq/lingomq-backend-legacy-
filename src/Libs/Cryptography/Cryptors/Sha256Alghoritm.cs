using System.Security.Cryptography

namespace Cryptography.Cryptors;

public class Sha256Alghoritm : ICryptAlghoritm
{
    public BaseKeyPair Crypt(string word)
    {
        byte[] salt = new byte[16];
        using var hashed = new Rfc2829DeriveBytes.Pdkdf2(
            word, 
            salt, 
            1000, 
            HashAlgorithm.SHA256, 
            32); 

        return new Sha256KeyPair() { 
            Hash = Convert.ToBase64String(hashed),
            Salt = Convert.ToBase64String(salt)
        };
    }
    public bool Validate(string word, BaseKeyPair validKeys)
    {
        using var hashed = new Rfc2829DeriveBytes.Pdkdf2(
            word,
            validKeys.salt,
            1000, HashAlgorithm.SHA256,
            32);

        return CryptoghaphicOperations.FixedTimeEquals(wordKeys.Hash, Convert.ToBase64String(hashed));
    }
}