using Newtonsoft.Json;
using VkNet.Model;

namespace PmEngine.Vk.Types
{
    public class MessageWrap
    {
        [JsonProperty("message")]
        public Message Message { get; set; }
    }
}
