using System.Collections;
using Loxodon.Framework.Binding.Contexts;

namespace SampleDevelop.Test
{
    public enum LoadState
    {
        none,
        loading,
        loaded,
    }

    public interface ISnkLoader
    {
        public LoadState mLoadState { get; }
        public string assetPath { get; }
        public void Load();
        public IEnumerator LoadAsync();
    }

    public interface ISnkView : ISnkViewControllable, ISnkLoader, IBindingContextOwner
    {
        public ISnkViewOwner mOwner { get; }
        public string mName { get; set; }
        public bool mVisibility { get; set; }
        public ISnkView mParentView { get; set; }

        public void Create();

    }
}