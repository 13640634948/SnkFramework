using SnkFramework.Core.Base;
using SnkFramework.Core.IoC;

namespace SnkFramework.Core
{
    public class Snk
    {
        private static ISnkIoCProvider _ioCProvider;
        public static ISnkIoCProvider IoCProvider 
            => _ioCProvider ??= SnkSingleton<ISnkIoCProvider>.Instance;
    }
}