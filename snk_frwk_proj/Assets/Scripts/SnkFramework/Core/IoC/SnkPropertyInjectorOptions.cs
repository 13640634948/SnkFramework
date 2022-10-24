// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace SnkFramework.Core.IoC
{
    public class SnkPropertyInjectorOptions : ISnkPropertyInjectorOptions
    {
        public SnkPropertyInjectorOptions()
        {
            InjectIntoProperties = SnkPropertyInjection.None;
            ThrowIfPropertyInjectionFails = false;
        }

        public SnkPropertyInjection InjectIntoProperties { get; set; }
        public bool ThrowIfPropertyInjectionFails { get; set; }

        private static ISnkPropertyInjectorOptions _SnkInjectProperties;

        public static ISnkPropertyInjectorOptions SnkInject
        {
            get
            {
                _SnkInjectProperties = _SnkInjectProperties ?? new SnkPropertyInjectorOptions()
                {
                    InjectIntoProperties = SnkPropertyInjection.SnkInjectInterfaceProperties,
                    ThrowIfPropertyInjectionFails = false
                };
                return _SnkInjectProperties;
            }
        }

        private static ISnkPropertyInjectorOptions _allProperties;

        public static ISnkPropertyInjectorOptions All
        {
            get
            {
                _allProperties = _allProperties ?? new SnkPropertyInjectorOptions()
                {
                    InjectIntoProperties = SnkPropertyInjection.AllInterfaceProperties,
                    ThrowIfPropertyInjectionFails = false
                };
                return _allProperties;
            }
        }
    }
}
