using Microsoft.EntityFrameworkCore;
using PmEngine.Core.Extensions;
using PmEngine.Core.Interfaces;
using PmEngine.Vk.Entities;

namespace PmEngine.Vk.Extensions
{
    public static class VkDataExtension
    {
        public static long? VkId(this IUserSession userSession)
        {
            return userSession.VkData()?.VkId;
        }

        public static VkDataUserEntity? VkData(this IUserSession userSession)
        {
            var vkUser = userSession.GetLocal<VkDataUserEntity?>("vkUserData");

            if (vkUser == null)
            {
                userSession.Services.InContext(async (context) =>
                {
                    vkUser = context.Set<VkDataUserEntity>().AsNoTracking().Include(u => u.Owner).Where(t => t.Owner.Id == userSession.Id).FirstOrDefault();
                }).Wait();

                userSession.SetLocal("vkUserData", vkUser);
            }

            return vkUser;
        }
    }
}