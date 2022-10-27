using VContainer;

namespace SnkFramework.Runtime.Core.Setup
{
    public partial class SnkSetupLifetimeScope
    {
        public virtual void InitializeSecondary(IContainerBuilder builder)
        {
            if (State != SnkSetupState.InitializedPrimary)
                return;
            State = SnkSetupState.InitializingSecondary;

            InitializeLastChance(builder);
            State = SnkSetupState.Initialized;
        }

        /// <summary>
        /// 尾初始化
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void InitializeLastChance(IContainerBuilder builder)
        {
        }
    }
}