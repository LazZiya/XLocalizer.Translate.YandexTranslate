XLocalizer.Translate.YandexTranslate

Instructions to use this package :

- This package requires Yandex API Key, must be obtained from https://tech.yandex.com/translate/
- Add the API key to user secrets :

````
{
  "XLocalizer.Translate": {
    "YandexTranslateApiKey": "xxx-yandex-translate-api-key-xxx"
  }
}
````

- Register in startup:
````
services.AddHttpClient<ITranslator, YandexTranslateService>();
````

Repository: https://github.com/LazZiya/XLocalizer.Translate.YandexTranslate