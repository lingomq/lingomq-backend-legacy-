using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Words.Application.Services.WordChecker.Models;

namespace Words.Application.Services.WordChecker;

public class WordChecker : IWordChecker
{
    private readonly Dictionary<string, string> _languageCodes = new Dictionary<string, string>()
        {
            { "english", "en-US" },
            { "russian", "ru-RU" },
            { "deutsch", "de-DE" },
            { "french", "fr-FR" },
            { "japanese", "ja-JP" }
        };
    private readonly static string _language = "language=";
    private readonly static string _params = "whitelist=&dictionary_id=&ai=0&";
    private readonly static string _uri = "https://api.textgears.com/spelling?" + _params;
    private readonly IConfiguration _configuration;
    public WordChecker(IConfiguration configuration) => _configuration = configuration;

    public async Task<string[]> SpellCorrector(string word, string language = "english")
    {
        string correct = word;
        string[] result = new string[] { correct };
        HttpClient client = new HttpClient();
        string uri = _uri + _language + _languageCodes.GetValueOrDefault(language) + "&key="
            + _configuration["Keys:SpellCheckerApiKey"] + "&text=" + word;

        using HttpResponseMessage response = await client.GetAsync(uri);

        if (response.IsSuccessStatusCode)
        {
            string json = await response.Content.ReadAsStringAsync();
            TextGearsResponseModel responseModel = JsonConvert.DeserializeObject<TextGearsResponseModel>(json);

            if (responseModel.Response!.Errors is not null && responseModel.Response!.Errors.Any() &&
                responseModel.Response!.Errors.First().Better is not null)
            {
                Console.WriteLine(responseModel.Response.Errors.First().Better!.First());
                Console.WriteLine(responseModel.Response.Errors.First().Better!);
                result = responseModel.Response.Errors.First().Better!.ToArray();
            }

        }

        return result;
    }
}
