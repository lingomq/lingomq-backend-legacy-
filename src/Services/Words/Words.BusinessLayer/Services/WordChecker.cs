using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Words.BusinessLayer.Contracts;
using Words.BusinessLayer.Models.TextGears;

namespace Words.BusinessLayer.Services
{
    public class WordChecker : IWordChecker
    {
        private readonly static string _language = "language=en-US";
        private readonly static string _params = "whitelist=&dictionary_id=&ai=0&" + _language;
        private readonly static string _uri = "https://api.textgears.com/spelling?" + _params;
        private readonly IConfiguration _configuration;
        public WordChecker(IConfiguration configuration) => _configuration = configuration;

        public async Task<string> SpellCorrector(string word, string language = "english")
        {
            string correct = word;
            HttpClient client = new HttpClient();
            string uri = _uri + "&key=" + _configuration["Keys:SpellCheckerApiKey"] + "&text=" + word;

            using HttpResponseMessage response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                TextGearsResponseModel responseModel = JsonConvert.DeserializeObject<TextGearsResponseModel>(json);

                if (responseModel.Response!.Errors is not null && responseModel.Response!.Errors.Any())
                {
                    Console.WriteLine(responseModel.Response.Errors.First().Better!.First());
                    Console.WriteLine(responseModel.Response.Errors.First().Better!);
                    correct = responseModel.Response.Errors.First().Better!.First();
                }
                    
            }

            return correct;
        }
    }
}
