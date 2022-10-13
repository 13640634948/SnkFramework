// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SnkFramework.Core.Exceptions;
using SnkFramework.Core.Logging;

namespace SnkFramework.Core.IoC
{
    public class SnkPropertyInjector : ISnkPropertyInjector
    {
        public virtual void Inject(object target, ISnkPropertyInjectorOptions options = null)
        {
            options = options ?? SnkPropertyInjectorOptions.All;

            if (options.InjectIntoProperties == SnkPropertyInjection.None)
                return;

            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var injectableProperties = FindInjectableProperties(target.GetType(), options);

            foreach (var injectableProperty in injectableProperties)
            {
                InjectProperty(target, injectableProperty, options);
            }
        }

        protected virtual void InjectProperty(object toReturn, PropertyInfo injectableProperty, ISnkPropertyInjectorOptions options)
        {
            object propertyValue;
            if (Snk.IoCProvider.TryResolve(injectableProperty.PropertyType, out propertyValue))
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
                    SnkLogHost.Default?.Log(LogLevel.Warning,
                        "IoC property injection skipped for {propertyName} on {typeName}",
                        injectableProperty.Name, toReturn.GetType().Name);
                }
            }
        }

        protected virtual IEnumerable<PropertyInfo> FindInjectableProperties(Type type, ISnkPropertyInjectorOptions options)
        {
            var injectableProperties = type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .Where(p => p.PropertyType.GetTypeInfo().IsInterface)
                .Where(p => p.IsConventional())
                .Where(p => p.CanWrite);

            switch (options.InjectIntoProperties)
            {
                case SnkPropertyInjection.SnkInjectInterfaceProperties:
                    injectableProperties = injectableProperties
                        .Where(p => p.GetCustomAttributes(typeof(SnkInjectAttribute), false).Any());
                    break;

                case SnkPropertyInjection.AllInterfaceProperties:
                    break;

                case SnkPropertyInjection.None:
                    SnkLogHost.Default?.Log(LogLevel.Error, "Internal error - should not call FindInjectableProperties with SnkPropertyInjection.None");
                    injectableProperties = new PropertyInfo[0];
                    break;

                default:
                    throw new SnkException("unknown option for InjectIntoProperties {0}", options.InjectIntoProperties);
            }
            return injectableProperties;
        }
    }
}
