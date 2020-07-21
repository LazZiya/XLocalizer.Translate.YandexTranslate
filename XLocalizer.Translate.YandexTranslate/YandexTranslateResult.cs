using System.Net;
using Newtonsoft.Json;

namespace XLocalizer.Translate.YandexTranslate
{
    /// <summary>
    /// Yandex translation result object
    /// </summary>
    public class YandexTranslateResult
    {
        /// <summary>
        /// HttpStatus code
        /// </summary>
        [JsonProperty("code")]
        public HttpStatusCode Code { get; set; }

        /// <summary>
        /// Translation direction e.g. en-ar
        /// </summary>
        [JsonProperty("lang")]
        public string LanguageDir { get; set; }

        /// <summary>
        /// Translated text
        /// </summary>
        [JsonProperty("text")]
        public string[] Text { get; set; }
    }
}
