// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
//using Microsoft.Extensions.Logging;
//using MvvmCross.Exceptions;
//using MvvmCross.Logging;
using SnkFramework.NuGet;
using SnkFramework.NuGet.Basic;
using SnkFramework.NuGet.Exceptions;

namespace MvvmCross.IoC
{
    public class MvxPropertyInjector : IMvxPropertyInjector
    {
        public virtual void Inject(object target, IMvxPropertyInjectorOptions options = null)
        {
            options = options ?? MvxPropertyInjectorOptions.All;

            if (options.InjectIntoProperties == MvxPropertyInjection.None)
                return;

            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var injectableProperties = FindInjectableProperties(target.GetType(), options);

            foreach (var injectableProperty in injectableProperties)
            {
                InjectProperty(target, injectableProperty, options);
            }
        }

        protected virtual void InjectProperty(object toReturn, PropertyInfo injectableProperty, IMvxPropertyInjectorOptions options)
        {
            object propertyValue = null;
            if (SnkSingleton<IMvxIoCProvider>.Instance?.TryResolve(injectableProperty.PropertyType, out propertyValue) == true)
            {
                try
                {
                    injectableProperty.SetValue(toReturn, propertyValue, null);
                }
                catch (TargetInvocationException invocation)
                {
                    throw new SnkIoCResolveException(invocation, "Failed to inject into {0} on {1}", injectableProperty.Name, toReturn.GetType().Name);
                }
            }
            else
            {
                if (options.ThrowIfPropertyInjectionFails)
                {
                    throw new SnkIoCResolveException("IoC property injection failed for {0} on {1}", injectableProperty.Name, toReturn.GetType().Name);
                }
                else
                {
                    SnkIoC.s_Logger?.Warning( "IoC property injection skipped for {propertyName} on {typeName}",
                        injectableProperty.Name, toReturn.GetType().Name);
                }
            }
        }

        protected virtual IEnumerable<PropertyInfo> FindInjectableProperties(Type type, IMvxPropertyInjectorOptions options)
        {
            var injectableProperties = type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .Where(p => p.PropertyType.GetTypeInfo().IsInterface)
                .Where(p => p.IsConventional())
                .Where(p => p.CanWrite);

            switch (options.InjectIntoProperties)
            {
                case MvxPropertyInjection.MvxInjectInterfaceProperties:
                    injectableProperties = injectableProperties
                        .Where(p => p.GetCustomAttributes(typeof(MvxInjectAttribute), false).Any());
                    break;

                case MvxPropertyInjection.AllInterfaceProperties:
                    break;

                case MvxPropertyInjection.None:
                    //MvxLogHost.Default?.Log(LogLevel.Error, "Internal error - should not call FindInjectableProperties with MvxPropertyInjection.None");
                    SnkIoC.s_Logger?.Error("Internal error - should not call FindInjectableProperties with MvxPropertyInjection.None");
                    injectableProperties = new PropertyInfo[0];
                    break;

                default:
                    throw new SnkException("unknown option for InjectIntoProperties {0}", options.InjectIntoProperties);
            }
            return injectableProperties;
        }
    }
}
