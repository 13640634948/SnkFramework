using System;
using SnkFramework.NuGet.Logging;

namespace SnkFramework.Runtime.Engine
{
    public class SnkUnityLogFactory : SnkLogFactory
    {
        protected override Func<string, SnkLogFactory, ISnkLog> loggerCreater
            => (s, factory) => new SnkUnityLog(s, factory);
    }
}