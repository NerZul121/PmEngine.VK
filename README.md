# PMEngine.VK
Модуль для работы с VK. В его основе лежит [VkNet](https://github.com/vknet/vk)  

На данный момент модуль еще достаточно сырой и нуждается в доработке.

## Используемые переменные

В модуле используются следующие переменные среды:
```
VK_GROUP_ID - ID группы в вк (без минуса!)
VK_TOKEN - Токен авторизации группы в ВК
```

## Подключение модуля

Для подключения модуля необходимо просто добавить его в список сервисов

```
builder.Services.AddVkModule();
```

## Создание контроллера

Для приема запросов от Vk необходимо добавить в приложение свой контроллер. Пример простого контроллера приведен ниже:

```
[ApiController]
[Route("[Controller]")]
public class VkController : ControllerBase
{
    private readonly IVkApi _vkApi;
    private readonly IServiceProvider _serviceProvider;
    private ILogger _logger;

    public VkController(IVkApi vkApi, IServiceProvider serviceProvider, ILogger logger)
    {
        _vkApi = vkApi;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Callback([FromBody] Updates updates)
    {
        switch (updates.Type)
        {
            case "confirmation":
                return Ok(Environment.GetEnvironmentVariable("VK_CONFIRMATION_CODE")); // ТУТ КОД ПОДТВЕРЖДЕНИЯ АВТОРИЗАЦИИ

            case "message_new":
                var vkController = new BaseVkConteoller();
                await vkController.Post(updates, _vkApi, _logger, _serviceProvider);

                break;
        }

        return Ok("Ok");
    }
}
```

Он использует класс ``BaseVkConteoller`` для обработки сообщений. Если вам необходимо обернуть обработку по-особому, то вы можете изменить логику контроллера на свою, опирась на код этого класса.

После настройки приложения не забудьте настроить паблик в вк на работу с этим приложением.

Если есть потребность в исползовании лонг-пуллинга, то пример реализации можно увидеть в репозитории VkNet.