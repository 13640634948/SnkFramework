using System.Collections;
using Loxodon.Framework.Binding.Contexts;
using SnkFramework.Mvvm.ViewModel;

namespace SampleDevelop.Test
{
    public interface ISnkView<TViewOwner, TViewModel> : ISnkView
        where TViewOwner : class, ISnkViewOwner
        where TViewModel : class, ISnkViewModel
    {
        public new TViewOwner mOwner { get; }
        public new TViewModel mViewModel { get; }
    }

    public interface ISnkView : ISnkViewControllable, IBindingContextOwner
    {
        public ISnkViewModel mViewModel { get; }
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