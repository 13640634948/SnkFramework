using System;
using System.Collections.Generic;
using SnkFramework.Core.Exceptions;

namespace SnkFramework.Core.Base
{
#nullable enable
    public abstract class SnkSingleton
        : IDisposable
    {
        ~SnkSingleton()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool isDisposing);

        private static readonly List<SnkSingleton> Singletons = new List<SnkSingleton>();

        protected SnkSingleton()
        {
            lock (Singletons)
            {
                Singletons.Add(this);
            }
        }

        public static void ClearAllSingletons()
        {
            lock (Singletons)
            {
                foreach (var s in Singletons)
                {
                    s.Dispose();
                }

                Singletons.Clear();
            }
        }
    }

    public abstract class SnkSingleton<TInterface>
        : SnkSingleton
        where TInterface : class
    {
        protected SnkSingleton()
        {
            if (Instance != null)
                throw new SnkException("You cannot create more than one instance of SnkSingleton");

            Instance = this as TInterface;
        }

        public static TInterface? Instance { get; private set; }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                Instance = null;
            }
        }
    }
#nullable restore
}