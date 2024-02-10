using LingoMqCryptography.Entities;

namespace LingoMqCryptography.Cryptors;
/// <summary>
/// The class is a contract on crypto alghoritms
/// </summary>
public interface ICryptAlghoritm
{
    public BaseKeyPair Crypt(string word);
    public bool Validate(string word, BaseKeyPair validKeys);
}