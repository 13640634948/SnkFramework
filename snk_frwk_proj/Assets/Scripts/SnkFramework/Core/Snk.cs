using SnkFramework.Core.Base;
using SnkFramework.Core.IoC;

namespace SnkFramework.Runtime
{
    public class Snk
    {
        public static ISnkIoCProvider IoCProvider => SnkSingleton<ISnkIoCProvider>.Instance;

    }
}