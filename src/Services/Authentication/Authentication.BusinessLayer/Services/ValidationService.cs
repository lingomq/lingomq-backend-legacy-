using Authentication.DomainLayer.Entities;
using Cryptography;
using Cryptography.Cryptors;
using Cryptography.Entities;

namespace Authentication.BusinessLayer.Services
{
    public class ValidationService
    {
        public static bool IsValidPassword(ref User user, string password)
        {
            Cryptor cryptor = new Cryptor(new Sha256Alghoritm());

            return cryptor.Validate(password,
                new BaseKeyPair() { Hash = user.PasswordHash, Salt = user.PasswordSalt });
        }
    }
}
