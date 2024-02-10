using Identity.Domain.Entities;
using LingoMqCryptography;
using LingoMqCryptography.Cryptors;
using LingoMqCryptography.Entities;

namespace Identity.Domain.Validations;
public class CredentialsValidator
{
    public static bool IsValidPassword(ref User user, string password)
    {
        Cryptor cryptor = new Cryptor(new Sha256Alghoritm());

        return cryptor.Validate(password,
            new BaseKeyPair() { Hash = user.PasswordHash, Salt = user.PasswordSalt });
    }
}
