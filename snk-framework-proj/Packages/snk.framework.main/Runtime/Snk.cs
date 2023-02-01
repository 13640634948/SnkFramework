using SnkFramework.IoC;
using SnkFramework.NuGet.Basic;

namespace SnkFramework.Runtime
{
    public class Snk
    {
        public static ISnkIoCProvider IoCProvider => SnkSingleton<ISnkIoCProvider>.Instance;
    }
}