using System.Collections;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Mvvm.Runtime
{
    namespace View
    {
        public abstract class SnkWindow : SnkUIBehaviour, ISnkWindow
        {
            public ISnkViewModel ViewModel { get; set; }
            public bool Visibility { get; set; }
            public bool Interactable { get; set; }
            public bool Activated { get; set; }

            public void Create(ISnkBundle bundle)
            {
                throw new System.NotImplementedException();
            }

            public IEnumerator Activate()
            {
                throw new System.NotImplementedException();
            }

            public IEnumerator Passivate()
            {
                throw new System.NotImplementedException();
            }

            public void Dismiss()
            {
                throw new System.NotImplementedException();
            }

            public ISnkView Current { get; }
            public ISnkView NavigatorPrev { get; }
            public ISnkView NavigatorNext { get; }

            public void Add(ISnkPage target)
            {
                throw new System.NotImplementedException();
            }

            public bool Remove(ISnkPage target)
            {
                throw new System.NotImplementedException();
            }

            public SnkWindowState WindowState { get; }
            public ISnkLayer Layer { get; }

            public IEnumerator Show(bool animated)
            {
                throw new System.NotImplementedException();
            }

            public IEnumerator Hide(bool animated)
            {
                throw new System.NotImplementedException();
            }

            public void AddPage(ISnkPage page)
            {
                throw new System.NotImplementedException();
            }

            public TViewModel AddPage<TViewModel>()
            {
                throw new System.NotImplementedException();
            }

            public Task<TViewModel> AddPageAsync<TViewModel>()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}