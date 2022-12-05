using UnityEngine;

namespace Loxodon.Framework.Binding.Contexts
{
    public interface IBindingContextOwner
    {
        public IBindingContext DataContext { get; set; }
    }
}