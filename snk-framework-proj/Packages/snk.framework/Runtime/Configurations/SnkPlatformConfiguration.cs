using SnkFramework.NuGet.Basic;
using SnkFramework.NuGet.Features.Configuration;

namespace SnkFramework.Runtime.Configurations
{
    public class SnkPlatformConfiguration : SnkPropertiesConfiguration
    {
        public SnkPlatformConfiguration(string assetPath) : base(new ResourceTextProvider(assetPath))
        {
            
        }

        public SnkPlatformConfiguration(ISnkTextProvider textProvider) : base(textProvider)
        {
        }
    }
}