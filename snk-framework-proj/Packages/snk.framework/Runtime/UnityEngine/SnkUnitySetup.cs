using SnkFramework.IoC;
using SnkFramework.NuGet.Features.Configuration;
using SnkFramework.NuGet.Preference;
using SnkFramework.NuGet.Features.Logging;
using SnkFramework.Runtime.Configurations;
using SnkFramework.Runtime.Core;
using SnkFramework.Runtime.Preference;

namespace SnkFramework.Runtime
{
    namespace Engine
    {
        public abstract class SnkUnitySetup : SnkSetup, ISnkUnitySetup
        {
            protected override ISnkLoggerProvider CreateLoggerProvider()
                => new SnkUnityLoggerProvider();

            protected virtual ISnkPreferenceSerializer CreatePreferenceSerializer()
                => new SnkUnityPrefSerializer();

            protected virtual ISnkPreferenceFactory CreatePreferenceFactory(ISnkPreferenceSerializer serializer)
                => new SnkPlayerPrefsPreferencesFactory(serializer);
            
            protected virtual void InitializePreference()
            {
                var serializer = CreatePreferenceSerializer();
                var preferenceFactory = CreatePreferenceFactory(serializer);
                SnkPreference.Register(preferenceFactory);
            }

            protected virtual void AddCustomConfigurations(SnkCompositeConfiguration configuration)
            {
                
            }

            protected virtual void InitializeConfiguration(ISnkIoCProvider iocProvider)
            {
                // 创建一个组合配置
                var configuration = new SnkCompositeConfiguration();
                
                // 加载平台配置文件
                configuration.AddConfiguration(new SnkPlatformConfiguration("platform")); 
                
                // 加载渠道配置文件
                configuration.AddConfiguration(new SnkChannelConfiguration("channel"));

                // 加载自定义配置文件
                AddCustomConfigurations(configuration);
                
                iocProvider.RegisterSingleton<ISnkConfiguration>(configuration);
            }

            protected override void RegisterDefaultDependencies(ISnkIoCProvider iocProvider)
            {
                base.RegisterDefaultDependencies(iocProvider);
                this.InitializeConfiguration(iocProvider);
                this.InitializePreference();
            }

            public virtual void PlatformInitialize()
            {
                
            }
        }

        public abstract class SnkUnitySetup<TSnkApplication> : SnkUnitySetup
            where TSnkApplication : SnkApplication
        {
            protected override ISnkApplication CreateApp(ISnkIoCProvider iocProvider) =>
                iocProvider.IoCConstruct<TSnkApplication>();
        }
    }
}