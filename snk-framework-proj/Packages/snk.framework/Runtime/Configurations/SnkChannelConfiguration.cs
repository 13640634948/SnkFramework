using SnkFramework.NuGet.Basic;
using SnkFramework.NuGet.Features.Configuration;

namespace SnkFramework.Runtime.Configurations
{
    public class SnkChannelConfiguration : SnkPropertiesConfiguration
    {
        public SnkChannelConfiguration(string assetPath) : base(new ResourceTextProvider(assetPath))
        {
            
        }

        public SnkChannelConfiguration(ISnkTextProvider textProvider) : base(textProvider)
        {
        }
    }
}