using SnkFramework.IoC;
using SnkFramework.NuGet.Preference;
using SnkFramework.NuGet.Features.Logging;
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

            protected virtual void InitializeConfiguration(ISnkIoCProvider iocProvider)
            {
                /*
                // 创建一个组合配置
                CompositeConfiguration configuration = new CompositeConfiguration(); 
                // 加载默认配置文件
                string defaultText = FileUtil.ReadAllText(Application.streamingAssetsPath + "/application.properties.txt"); 
                configuration.AddConfiguration(new PropertiesConfiguration(defaultText)); 
#if UNITY_EDITOR 
                string text = FileUtil.ReadAllText(Application.streamingAssetsPath + "/application.editor.properties.txt"); 
#elif UNITY_ANDROID 
                string text = FileUtil.ReadAllText(Application.streamingAssetsPath + "/application.android.properties.txt"); 
#elif UNIYT_IOS 
                string text = FileUtil.ReadAllText(Application.streamingAssetsPath + "/application.ios.properties.txt"); 

    #endif 
                // 加载当前平台的配置文件
                configuration.AddConfiguration(new PropertiesConfiguration(text)); 
                // 注册配置文件到容器中
                container.Register<IConfiguration>(configuration);
                */
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