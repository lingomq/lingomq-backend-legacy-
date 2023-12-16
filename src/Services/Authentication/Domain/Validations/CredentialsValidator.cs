using Cryptography;
using Cryptography.Cryptors;
using Cryptography.Entities;
using Authentication.Domain.Entities;

namespace Domain.Validations;
public class CredentialsValidator
{
    public static bool IsValidPassword(ref User user, string password)
    {
        Cryptor cryptor = new Cryptor(new Sha256Alghoritm());

        return cryptor.Validate(password,
            new BaseKeyPair() { Hash = user.PasswordHash, Salt = user.PasswordSalt });
    }
}
