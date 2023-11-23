using Microsoft.Extensions.DependencyInjection;
using PmEngine.Core.Interfaces;
using PmEngine.Core.BaseClasses;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet;

namespace PmEngine.Vk
{
    public class VkModule : BaseModuleRegistrator
    {
    }

    public static class VkModuleExt
    {
        public static IServiceCollection AddVkModule(this IServiceCollection services)
        {
            services.AddSingleton<IModuleRegistrator>(new VkModule());
            services.AddTransient(typeof(IDataContext), typeof(VkContext));

            services.AddScoped(typeof(IOutputManager), typeof(VkOutputManager));
            services.AddScoped(typeof(IVkOutputManager), typeof(VkOutputManager));

            var api = new VkApi();
            
            services.AddSingleton<IVkApi>(sp =>
            {
                api.Authorize(new ApiAuthParams { AccessToken = Environment.GetEnvironmentVariable("VK_TOKEN") });
                return api;
            });

            return services;
        }
    }
}
