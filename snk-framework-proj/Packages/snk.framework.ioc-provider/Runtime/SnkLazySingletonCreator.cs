// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using SnkFramework.NuGet;
using SnkFramework.NuGet.Basic;

namespace SnkFramework.IoC
{
    public class SnkLazySingletonCreator
    {
        private readonly object _lockObject = new object();
        private readonly Type _type;

        private object _instance;

        public object Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (_lockObject)
                {
                    _instance = _instance ?? SnkSingleton<ISnkIoCProvider>.Instance.IoCConstruct(_type);
                    return _instance;
                }
            }
        }

        public SnkLazySingletonCreator(Type type)
        {
            _type = type;
        }
    }
}
