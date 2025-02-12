// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace SnkFramework.IoC
{
    public interface ISnkIocOptions
    {
        bool TryToDetectSingletonCircularReferences { get; }
        bool TryToDetectDynamicCircularReferences { get; }
        bool CheckDisposeIfPropertyInjectionFails { get; }
        Type PropertyInjectorType { get; }
        ISnkPropertyInjectorOptions PropertyInjectorOptions { get; }
    }
}
