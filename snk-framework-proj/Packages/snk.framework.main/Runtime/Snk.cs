using SnkFramework.IoC;
using SnkFramework.NuGet.Basic;

namespace DefaultNamespace
{
    public class Snk
    {
        public static ISnkIoCProvider IoCProvider => SnkSingleton<ISnkIoCProvider>.Instance;
    }
}