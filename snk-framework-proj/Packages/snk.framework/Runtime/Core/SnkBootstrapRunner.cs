using System;
using System.Reflection;
using SnkFramework.IoC;

namespace SnkFramework.Runtime.Core
{
    public class SnkBootstrapRunner
    {
        public virtual void Run(Assembly assembly)
        {
            var types = assembly.CreatableTypes()
                .Inherits<ISnkBootstrapAction>();

            foreach (var type in types)
            {
                Run(type);
            }
        }

        protected virtual void Run(Type type)
        {
            if(type == null)
                throw new ArgumentNullException(nameof(type));
            try
            {
                var toRun = Activator.CreateInstance(type);
                if (toRun is not ISnkBootstrapAction bootstrapAction)
                {
                    SnkLogHost.Default?.InfoFormat("Could not run startup task {TypeName} - it's not a startup task", type.Name);
                    return;
                }

                bootstrapAction.Run();
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                // pokemon handling
                SnkLogHost.Default?.Exception(exception, "Error running startup task {TypeName}", type.Name);
            }
        }
    }
}