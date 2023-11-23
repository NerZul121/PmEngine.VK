using PmEngine.Core.Interfaces;
using VkNet.Abstractions;

namespace PmEngine.Vk
{
    public interface IVkOutputManager : IOutputManager
    {
        string Storage { get; set; }
        IVkApi VkApi { get; }

        Task DeleteMessage(int messageId);
        Task EditContent(int messageId, string content, INextActionsMarkup? nextActions = null, IEnumerable<object>? media = null, IActionArguments? additionals = null);
        Task<int> ShowContent(string content, INextActionsMarkup? nextActions = null, IEnumerable<object>? media = null, IActionArguments? additionals = null);
    }
}