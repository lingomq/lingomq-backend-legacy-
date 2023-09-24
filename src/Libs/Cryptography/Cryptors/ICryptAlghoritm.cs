using Cryptography.Entities;

namespace Cryptography.Cryptors;

public interface ICryptAlghoritm
{
    public BaseKeyPair Crypt(string word);
    public bool Validate(string word, BaseKeyPair validKeys);
}