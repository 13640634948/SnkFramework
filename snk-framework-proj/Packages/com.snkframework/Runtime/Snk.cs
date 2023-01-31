using MvvmCross.IoC;
using SnkFramework.NuGet.Basic;

namespace DefaultNamespace
{
    public class Snk
    {
        public static IMvxIoCProvider IoCProvider => SnkSingleton<IMvxIoCProvider>.Instance;
    }
}