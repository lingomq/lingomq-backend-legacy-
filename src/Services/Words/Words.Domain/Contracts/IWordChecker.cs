namespace Words.Domain.Contracts;
public interface IWordChecker
{
    // TODO: In the next time realize it
    // bool? IsCorrectTranslate(string word, string translated);
    Task<string> SpellCorrector(string word, string language = "english");
}
