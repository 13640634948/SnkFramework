using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace MvvmCross.Demo
{
#nullable enable
    public abstract class MvxUnityApplication : MvxApplication, IMvxUnityApplication
    {
    }

    public abstract class MvxUnityApplication<THint> : MvxUnityApplication, IMvxUnityApplication<THint>
    {
        public abstract Task<THint> Startup(THint hint);
    }
#nullable restore
}