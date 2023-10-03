namespace Words.BusinessLayer.Contracts
{
    public interface IWordChecker
    {
        bool? IsCorrectTranslate(string word, string translated);
        string SpellChecker(string word);
    }
}
