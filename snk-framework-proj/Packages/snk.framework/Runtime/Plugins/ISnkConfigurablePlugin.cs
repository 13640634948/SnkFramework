namespace SnkFramework.Plugins
{
    public interface ISnkConfigurablePlugin : ISnkPlugin
    {
        void Configure(ISnkPluginConfiguration configuration);
    }
}