// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace SnkFramework.IoC
{
    public interface ISnkPropertyInjector
    {
        void Inject(object target, ISnkPropertyInjectorOptions options = null);
    }
}
