using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace XLocalizer.Translate.YandexTranslate
{
    /// <summary>
    /// Yandex translate service
    /// </summary>
    public class YandexTranslateService : ITranslator
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly string _yandexTranslateApiKey;

        /// <summary>
        /// Initialize yandex translate service
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public YandexTranslateService(HttpClient httpClient, IConfiguration configuration, ILogger<YandexTranslateService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _yandexTranslateApiKey = configuration["XLocalizer.TranslationServices:YandexTranslateApiKey"];
        }

        /// <summary>
        /// Service name
        /// </summary>
        public string ServiceName => "Yandex Translate";

        /// <summary>
        /// Run async translation task
        /// </summary>
        /// <param name="source">Source language e.g. en</param>
        /// <param name="target">Target language e.g. tr</param>
        /// <param name="text">Text to be translated</param>
        /// <param name="format">Text format: html or text</param>
        /// <returns><see cref="TranslationResult"/></returns>
        public async Task<TranslationResult> TranslateAsync(string source, string target, string text, string format)
        {

            if (string.IsNullOrWhiteSpace(_yandexTranslateApiKey))
            {
                throw new NullReferenceException(nameof(_yandexTranslateApiKey));
            }

            try
            {
                var response = await _httpClient.GetAsync($"https://translate.yandex.net/api/v1.5/tr.json/translate?key={_yandexTranslateApiKey}&text={text}&lang={source}-{target}&format={format}");
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Response: {ServiceName} - {response.StatusCode}");
                /*
                 * Sample response content for "Back" translation
                 * {
                 *     "code": 200,
                 *     "lang": "en-ar",
                 *     "text": ["مرة أخرى"]
                 * } 
                 * */

                var responseDto = JsonConvert.DeserializeObject<YandexTranslateResult>(responseContent);

                return new TranslationResult
                {
                    Text = responseDto.Text[0],
                    StatusCode = response.StatusCode,
                    Target = target,
                    Source = source
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"Error {ServiceName} - {e.Message}");
            }

            return new TranslationResult
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Text = text,
                Target = target,
                Source = source
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="text"></param>
        /// <param name="translation"></param>
        /// <returns></returns>
        public bool TryTranslate(string source, string target, string text, out string translation)
        {
            var trans = Task.Run(() => TranslateAsync(source, target, text, "text")).GetAwaiter().GetResult();

            if (trans.StatusCode == HttpStatusCode.OK)
            {
                translation = trans.Text;
                return true;
            }

            translation = text;
            return false;
        }
    }
}
