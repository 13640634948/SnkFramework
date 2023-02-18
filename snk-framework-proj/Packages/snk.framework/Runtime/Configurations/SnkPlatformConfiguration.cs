using SnkFramework.NuGet.Features.Configuration;

namespace SnkFramework.Runtime.Configurations
{
    public class SnkPlatformConfiguration : SnkPropertiesConfiguration
    {
        public SnkPlatformConfiguration() : base(new ResourceTextProvider())
        {
            
        }

        public SnkPlatformConfiguration(ISnkTextProvider textProvider) : base(textProvider)
        {
        }
    }
}