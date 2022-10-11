using Loxodon.Framework.Binding.Contexts;

namespace SampleDevelop.Test
{
    public interface ISnkView : ISnkViewControllable, ISnkLoader, IBindingContextOwner
    {
        public ISnkViewOwner mOwner { get; }
        public string mName { get; set; }
        public bool mVisibility { get; set; }
        public ISnkView mParentView { get; set; }

        public void Create();

    }
}