// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace SnkFramework.IoC
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

        private static ISnkPropertyInjectorOptions _snkInjectProperties;

        public static ISnkPropertyInjectorOptions SnkInject
        {
            get
            {
                _snkInjectProperties = _snkInjectProperties ?? new SnkPropertyInjectorOptions()
                {
                    InjectIntoProperties = SnkPropertyInjection.MvxInjectInterfaceProperties,
                    ThrowIfPropertyInjectionFails = false
                };
                return _snkInjectProperties;
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
