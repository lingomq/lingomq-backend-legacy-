using Cryptography.Entities;
using System.Security.Cryptography;
using System.Text;

namespace Cryptography.Cryptors;

public class Sha256Alghoritm : ICryptAlghoritm
{
    public BaseKeyPair Crypt(string word)
    {
        byte[] salt = new byte[16];
        var hashed = new Rfc2898DeriveBytes(
            word, 
            salt,
            10000,
            HashAlgorithmName.SHA256);

        byte[] hash = hashed.GetBytes(32);

        return new Sha256KeyPair() {
            Hash = Convert.ToBase64String(hash),
            Salt = Convert.ToBase64String(salt)
        };
    }
    public bool Validate(string word, BaseKeyPair validKeys)
    {
        var hashed = new Rfc2898DeriveBytes(
            word,
            Encoding.UTF8.GetBytes(validKeys.Salt!),
            10000,
            HashAlgorithmName.SHA256);

        byte[] hash = hashed.GetBytes(32);

        return Convert.ToBase64String(hash).Equals(validKeys.Hash);
    }
}