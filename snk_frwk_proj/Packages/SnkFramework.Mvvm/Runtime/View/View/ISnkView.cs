using System.Collections;
using Loxodon.Framework.Binding.Contexts;

namespace SampleDevelop.Test
{
    public interface ISnkView : ISnkViewControllable, IBindingContextOwner
    {
        public ISnkViewOwner mOwner { get; }
        public string mName { get; set; }
        public bool mVisibility { get; set; }
        public ISnkView mParentView { get; set; }

        public void Create();
        public bool mViewOwnerLoaded { get; }
        public string mAssetPath { get; }
        public void LoadViewOwner();
        public IEnumerator LoadViewOwnerAsync();

        public void UnloadViewOwner();
    }
}