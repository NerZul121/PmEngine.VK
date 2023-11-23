using Microsoft.Extensions.DependencyInjection;
using PmEngine.Core.Entities;
using PmEngine.Core.Enums;
using PmEngine.Core.Interfaces;
using PmEngine.Core;
using VkNet.Abstractions;
using Microsoft.Extensions.Logging;
using PmEngine.Vk.Entities;
using Newtonsoft.Json;
using PmEngine.Vk.Types;
using PmEngineVk.Types;
using Microsoft.EntityFrameworkCore;
using VkNet.Model;

namespace PmEngine.Vk
{
    public class BaseVkConteoller
    {
        public virtual async Task<bool> Post(Updates update, IVkApi client, ILogger logger, IServiceProvider serviceProvider)
        {
            var str = (string?)update.Object?.ToString();
            var msg = JsonConvert.DeserializeObject<MessageWrap>(str ?? "")?.Message;
            VkDataUserEntity? vkUser = null;
            UserEntity? user = null;
            IUserSession? session;

            try
            {
                if (msg?.FromId is null)
                    return false;

                vkUser = await GetOrCreateUser(msg.FromId ?? -1, serviceProvider);

                user = vkUser?.Owner;

                if (vkUser is null || user is null)
                    return false;

                session = await serviceProvider.GetRequiredService<IServerSession>().GetUserSession(user.Id, u => u.SetDefaultOutput<IVkOutputManager>());
                session.SetDefaultOutput<IVkOutputManager>();
                return await UserProcess(msg, session, client, logger, serviceProvider);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }

            return false;
        }

        public virtual void UserRightsVerify(IUserSession user)
        {
            if (user.CachedData.UserType == (int)UserType.Banned)
                throw new Exception("Sorry, your account has blocked :(");
        }

        public virtual async Task<VkDataUserEntity> GetOrCreateUser(long vkId, IServiceProvider serviceProvider, long? userId = null)
        {
            VkDataUserEntity? vkUser = null;

            await serviceProvider.GetRequiredService<IContextHelper>().InContext(async (context) =>
            {
                vkUser = await context.Set<VkDataUserEntity>().AsNoTracking().Include(u => u.Owner).FirstOrDefaultAsync(p => p.VkId == vkId);

                if (vkUser is null)
                {
                    vkUser = new VkDataUserEntity() { VkId = vkId };
                    if (userId is not null)
                        vkUser.Owner = context.Set<UserEntity>().First(u => u.Id == userId);
                    else
                        vkUser.Owner = new();

                    await context.Set<VkDataUserEntity>().AddAsync(vkUser);

                    await context.SaveChangesAsync();
                    context.Entry(vkUser).State = EntityState.Detached;
                }

                vkUser = await context.Set<VkDataUserEntity>().AsNoTracking().Include(u => u.Owner).FirstOrDefaultAsync(p => p.VkId == vkId);
            });

            return vkUser;
        }

        public virtual async Task<bool> UserProcess(Message msg, IUserSession session, IVkApi client, ILogger logger, IServiceProvider serviceProvider)
        {
            UserRightsVerify(session);

            logger.LogInformation($"New message from {msg.FromId}: {msg.Text}");

            session.SetDefaultOutput<IVkOutputManager>();

            var stringed = session.NextActions is not null ? session.NextActions.NumeredDuplicates().GetFloatNextActions() : Enumerable.Empty<IActionWrapper>();

            var processor = serviceProvider.GetRequiredService<IEngineProcessor>();

            if (msg.Attachments != null && msg.Attachments.Any())
            {
                var fileUid = msg.Attachments.First();

                if (session.InputAction != null)
                {
                    if (msg.Attachments.First().Instance is Photo)
                        session.InputAction.Arguments.Set("inputData", ((Photo)msg.Attachments.First().Instance).Sizes.OrderByDescending(s => s.Width).ThenByDescending(s => s.Height).First().Url.ToString());

                    await processor.ActionProcess(session.InputAction, session, session.InputAction.Arguments);
                }

                return true;
            }

            if (String.IsNullOrEmpty(msg.Text))
                return false;

            var act = stringed.FirstOrDefault(a => a.DisplayName == msg.Text);

            if (act is not null)
            {
                await processor.ActionProcess(act, session, act.Arguments);
                return true;
            }
            else if (msg.Text.StartsWith("/"))
            {
                var cmdmngr = serviceProvider.GetServices<IManager>().First(m => m.GetType() == typeof(CommandManager)) as CommandManager;
                await cmdmngr.DoCommand(msg.Text, session);
                return true;
            }
            else if (session.InputAction != null)
            {
                session.InputAction.Arguments.Set("inputData", msg.Text);
                await processor.ActionProcess(session.InputAction, session, session.InputAction.Arguments);
                return true;
            }

            return false;
        }
    }
}
