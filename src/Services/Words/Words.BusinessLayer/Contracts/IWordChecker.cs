namespace Words.BusinessLayer.Contracts
{
    public interface IWordChecker
    {
        bool? IsCorrectTranslate(string word, string translated);
        Task<string> SpellCorrector(string word);
    }
}
