using SnkFramework.Mvvm.Runtime;

namespace SnkFramework
{
    public class MvvmUI
    {
        public static ISnkMvvmService CreateService<TInstaller>()
            where TInstaller : SnkMvvmInstaller, new()
        {
            TInstaller installer = new TInstaller();
            return installer.Install();
        }
    }
}