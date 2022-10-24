// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace SnkFramework.Core.IoC
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
                    _instance = _instance ?? Snk.IoCProvider.IoCConstruct(_type);
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
