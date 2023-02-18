using SnkFramework.NuGet.Features.Configuration;

namespace SnkFramework.Runtime.Configurations
{
    public class SnkChannelConfiguration : SnkPropertiesConfiguration
    {
        public SnkChannelConfiguration() : base(new ResourceTextProvider())
        {
            
        }

        public SnkChannelConfiguration(ISnkTextProvider textProvider) : base(textProvider)
        {
        }
    }
}