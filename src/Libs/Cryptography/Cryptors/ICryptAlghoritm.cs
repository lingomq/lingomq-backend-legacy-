using Cryptography.Entities;

namespace Cryptography.Cryptors;

public interface ICryptAlghoritm
{
    public BaseKeyPair Crypt(string word);
}