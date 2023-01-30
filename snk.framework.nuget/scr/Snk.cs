using System.Collections.Generic;

using SnkFramework.NuGet.Core;
using SnkFramework.NuGet.Exceptions;

namespace SnkFramework.NuGet
{
    public class Snk
    {
        private static ISnkSetup s_Setup;

        private static readonly Dictionary<string, object> dict = new Dictionary<string, object>();

        public static void Launcher<TSetup>() where TSetup : class, ISnkSetup, new()
        {
            s_Setup = new TSetup();
            Set(s_Setup.CreateJsonParser());
            Set(s_Setup.CreateCompressor());
            Set(s_Setup.CreateCodeGenerator());
            Set(s_Setup.CreateLogger());
        }

        public static T Get<T>() where T : class
        {
            var key = typeof(T).FullName;
            if (dict.TryGetValue(key, out var target) == false)
            {
                throw new SnkException(string.Format("found out {0} in Snk.IOC", typeof(T)));
            }
            return target as T;
        }

        public static void Set<T>(T target)
        {
            Set(typeof(T).FullName, target);
        }

        public static void Set(string key, object target)
        {
            if (dict.TryGetValue(key, out var value) == true)
                return;
            dict.Add(key, target);
        }
    }
}

