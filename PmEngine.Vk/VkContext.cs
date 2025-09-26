using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PmEngine.Core.BaseClasses;
using PmEngine.Core.Interfaces;

namespace PmEngine.Vk
{
    public class VkContext : BaseContext
    {
        public VkContext(IEngineConfigurator configurator = null) : base(configurator)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
            base.OnConfiguring(optionsBuilder);
        }
    }
}