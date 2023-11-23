using Microsoft.Extensions.Logging;
using PmEngine.Core.Interfaces;
using VkNet.Abstractions;
using VkNet.Model;
using PmEngine.Vk.Extensions;
using PmEngine.Core;
using System.Net.Http.Headers;
using System.Text;
using System.Net;
using VkNet.Enums.StringEnums;

namespace PmEngine.Vk
{
    public class VkOutputManager : IVkOutputManager
    {
        public string Storage { get; set; } = "";

        private ILogger _logger;
        private IUserScopeData _userData;
        public IVkApi VkApi { get; }

        /// <summary>
        /// Инициализация аутпата
        /// </summary>
        /// <param name="logger">логгер</param>
        /// <param name="client">телеграммный клиент</param>
        public VkOutputManager(ILogger logger, IVkApi client, IUserScopeData userData)
        {
            _logger = logger;
            VkApi = client;
            _userData = userData;
        }

        public async Task DeleteMessage(int messageId)
        {
            await VkApi.Messages.DeleteAsync(new ulong[] { (ulong)messageId });
        }

        public Task EditContent(int messageId, string content, INextActionsMarkup? nextActions = null, IEnumerable<object>? media = null, IActionArguments? additionals = null)
        {
            return Task.CompletedTask;
        }

        public async Task<int> ShowContent(string content, INextActionsMarkup? nextActions = null, IEnumerable<object>? media = null, IActionArguments? additionals = null)
        {
            var message = new MessagesSendParams()
            {
                UserId = _userData.Owner.VkId(),
                Message = content,
                RandomId = Randomizer.Next(0, 999999)
            };

            if (nextActions != null)
            {
                var btns = nextActions.GetNextActions().Where(s => s.Any()).Select(s => s.Select(s => new MessageKeyboardButton()
                {
                    Action = new MessageKeyboardButtonAction()
                    {
                        Type = KeyboardButtonActionType.Text,
                        Label = s.DisplayName
                    },

                    Color = KeyboardButtonColor.Primary
                }));

                var keyboard = new MessageKeyboard()
                {
                    Buttons = btns,
                    Inline = nextActions.InLine,
                };

                message.Keyboard = keyboard;
            }

            if (media != null && media.Any())
            {
                var uploadServer = VkApi.Photo.GetMessagesUploadServer(Convert.ToInt64(Environment.GetEnvironmentVariable("VK_GROUP_ID")));
                var attachmenst = new List<Photo>();

                foreach (var med in media)
                {
                    var stringed = med.ToString();

                    if (stringed is null)
                        continue;

                    if (stringed.StartsWith("http"))
                    {
                        var response = await UploadFile(uploadServer.UploadUrl, GetWebBytes(stringed), stringed.Split('.').Last());
                        var attachment = await VkApi.Photo.SaveMessagesPhotoAsync(response);
                        attachmenst.AddRange(attachment.ToList());
                    }
                    else
                    {
                        try
                        {
                            if (stringed.EndsWith(".jpg"))
                            {
                                var response = await UploadFile(uploadServer.UploadUrl, GetBytes(stringed), stringed.Split('.').Last());
                                var attachment = VkApi.Photo.SaveMessagesPhoto(response);
                                attachmenst.AddRange(attachment.ToList());
                            }
                            else
                            {
                                var bytes = Convert.FromBase64String(stringed);
                                using var contents = new StreamContent(new MemoryStream(bytes));
                                var response = await UploadFile(uploadServer.UploadUrl, await contents.ReadAsByteArrayAsync(), "png");
                                var attachment = VkApi.Photo.SaveMessagesPhoto(response);
                                attachmenst.AddRange(attachment.ToList());
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.LogError($"Ошибка при загрузке файла {stringed}: {e}");
                        }
                    }
                }

                message.Attachments = attachmenst;
            }

            return (int)await VkApi.Messages.SendAsync(message);
        }

        public async Task<string> UploadFile(string serverUrl, byte[] data, string fileExtension)
        {
            // Создание запроса на загрузку файла на сервер
            using (var client = new HttpClient())
            {
                var requestContent = new MultipartFormDataContent();
                var content = new ByteArrayContent(data);
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                requestContent.Add(content, "file", $"file.{fileExtension}");

                var response = client.PostAsync(serverUrl, requestContent).Result;
                return Encoding.Default.GetString(await response.Content.ReadAsByteArrayAsync());
            }
        }

        public byte[] GetWebBytes(string fileUrl)
        {
            using (var webClient = new WebClient())
            {
                return webClient.DownloadData(fileUrl);
            }
        }

        public byte[] GetBytes(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }
    }
}