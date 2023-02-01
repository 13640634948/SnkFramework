namespace SnkFramework.Plugins
{
    public interface IMvxConfigurablePlugin : ISnkPlugin
    {
        void Configure(IMvxPluginConfiguration configuration);
    }
}