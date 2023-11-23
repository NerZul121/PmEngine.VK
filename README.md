# PMEngine.VK
������ ��� ������ � VK. � ��� ������ ����� [VkNet](https://github.com/vknet/vk)  

�� ������ ������ ������ ��� ���������� ����� � ��������� � ���������.

## ������������ ����������

� ������ ������������ ��������� ���������� �����:
```
VK_GROUP_ID - ID ������ � �� (��� ������!)
VK_TOKEN - ����� ����������� ������ � ��
```

## ����������� ������

��� ����������� ������ ���������� ������ �������� ��� � ������ ��������

```
builder.Services.AddVkModule();
```

## �������� �����������

��� ������ �������� �� Vk ���������� �������� � ���������� ���� ����������. ������ �������� ����������� �������� ����:

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
                return Ok(Environment.GetEnvironmentVariable("VK_CONFIRMATION_CODE")); // ��� ��� ������������� �����������

            case "message_new":
                var vkController = new BaseVkConteoller();
                await vkController.Post(updates, _vkApi, _logger, _serviceProvider);

                break;
        }

        return Ok("Ok");
    }
}
```

�� ���������� ����� ``BaseVkConteoller`` ��� ��������� ���������. ���� ��� ���������� �������� ��������� ��-�������, �� �� ������ �������� ������ ����������� �� ����, ������� �� ��� ����� ������.

����� ��������� ���������� �� �������� ��������� ������ � �� �� ������ � ���� �����������.

���� ���� ����������� � ������������ ����-��������, �� ������ ���������� ����� ������� � ����������� VkNet.