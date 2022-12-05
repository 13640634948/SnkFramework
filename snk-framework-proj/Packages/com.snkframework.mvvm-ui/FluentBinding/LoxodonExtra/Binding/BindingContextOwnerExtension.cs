using System;
using Loxodon.Framework.Binding.Binders;
using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.Contexts;

namespace Loxodon.Framework.Binding.Contexts
{
    public static class BindingContextOwnerExtension
    {
        
        private static IBinder binder;
        public static IBinder Binder
        {
            get
            {
                if (binder == null)
                    binder = Context.GetApplicationContext().GetService<IBinder>();

                if (binder == null)
                    throw new Exception("Data binding service is not initialized,please create a BindingServiceBundle service before using it.");

                return binder;
            }
        }

        public static IBindingContext BindingContext(this IBindingContextOwner bindingContextOwner)
        {
            if (bindingContextOwner == null)
                return null;

            IBindingContext bindingContext = bindingContextOwner.DataContext;
            if (bindingContext == null)
            {
                bindingContext = new BindingContext(bindingContextOwner, Binder);
                bindingContextOwner.DataContext = bindingContext;
            }
            return bindingContext;
        }

        
        public static BindingSet<TBindingContextOwner, TSource> CreateBindingSet<TBindingContextOwner, TSource>(this TBindingContextOwner behaviour, TSource dataContext) 
            where TBindingContextOwner : class, IBindingContextOwner
        {
            IBindingContext context = behaviour.BindingContext();
            context.DataContext = dataContext;
            return new BindingSet<TBindingContextOwner, TSource>(context, behaviour);
        }

    }
}