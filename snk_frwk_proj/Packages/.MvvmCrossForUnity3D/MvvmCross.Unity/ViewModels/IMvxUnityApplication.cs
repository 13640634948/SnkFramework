using MvvmCross.ViewModels;

namespace MvvmCross.Demo
{
#nullable enable
    public interface IMvxUnityApplication : IMvxApplication
    {
    }

    public interface IMvxUnityApplication<THint> : IMvxUnityApplication, IMvxApplication<THint>
    {
    }
#nullable restore
}