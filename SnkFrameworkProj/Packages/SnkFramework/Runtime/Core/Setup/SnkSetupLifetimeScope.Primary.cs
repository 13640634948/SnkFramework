using VContainer;

namespace SnkFramework.Runtime.Core.Setup
{
    public partial class SnkSetupLifetimeScope
    {
        /// <summary>
        /// 主要初始化（UnityThread）
        /// </summary>
        /// <param name="builder"></param>
        public virtual void InitializePrimary(IContainerBuilder builder)
        {
            if (State != SnkSetupState.Uninitialized)
                return;
            State = SnkSetupState.InitializingPrimary;

            InitializeLoggingServices(builder);

            RegisterDefaultSetupDependencies(builder);
            InitializeBasePlatform(builder);
            InitializeFirstChance(builder);
            State = SnkSetupState.InitializedPrimary;
        }
      
        /// <summary>
        /// 初始化日志服务
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void InitializeLoggingServices(IContainerBuilder builder)
        {
            
        }

        /// <summary>
        /// 注册框架设置
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void RegiestFrameworkSetting(IContainerBuilder builder)
        {
            
        }

        /// <summary>
        /// 默认的安装器依赖
        /// </summary>
        protected virtual void RegisterDefaultSetupDependencies(IContainerBuilder builder)
        {
            RegiestFrameworkSetting(builder);
        }

        /// <summary>
        /// 初始化各个平台
        /// </summary>
        protected virtual void InitializeBasePlatform(IContainerBuilder builder)
        {
            
        }

        /// <summary>
        /// 首初始化
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void InitializeFirstChance(IContainerBuilder builder)
        {
            // always the very first thing to get initialized
            // base class implementation is empty by default
            // 第一件要初始化的事，默认为空实现，需要的自行重写
        }
    }
}