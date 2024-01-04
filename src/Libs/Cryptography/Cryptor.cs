using Cryptography.Entities;
using Cryptography.Cryptors;

namespace Cryptography;

public class Cryptor
{
    private ICryptAlghoritm _cryptAlghoritm;
    public Cryptor(ICryptAlghoritm alghoritm) =>
        _cryptAlghoritm = alghoritm;
    public BaseKeyPair Crypt(string word) =>
        _cryptAlghoritm.Crypt(word);
    public bool Validate(string word, BaseKeyPair validKeys) =>
        _cryptAlghoritm.Validate(word, validKeys);
}