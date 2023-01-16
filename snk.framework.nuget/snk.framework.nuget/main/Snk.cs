﻿using snk.framework.nuget.core;
using System.Collections.Generic;

namespace snk.framework.nuget
{
    public class Snk
    {
        private static ISnkSetup s_Setup;
        public static void Launcher<TSetup>() where TSetup : class, ISnkSetup, new()
        {
            s_Setup = new TSetup();
            Set(s_Setup.CreateJsonParser());
            Set(s_Setup.CreateCompressor());
            Set(s_Setup.CreateCodeGenerator());
            Set(s_Setup.CreateComparer());
        }

        private static readonly Dictionary<string, object> dict = new Dictionary<string, object>();

        public static T Get<T>() where T : class
        {
            var key = typeof(T).FullName;
            if (dict.TryGetValue(key, out var target) == false)
            {
                throw new System.Exception(string.Format("found out {0} in Snk.IOC", typeof(T)));
            }
            return target as T;
        }

        public static void Set<T>(T target)
        {
            Set(typeof(T).FullName, target);
        }

        public static void Set(string key, object target)
        {
            dict.Add(key, target);
        }
    }
}

