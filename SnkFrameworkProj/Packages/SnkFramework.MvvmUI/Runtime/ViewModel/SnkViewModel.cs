using System.Threading.Tasks;
using Loxodon.Framework.Observables;
using SnkFramework.Mvvm.Runtime.Base;

namespace SnkFramework.Mvvm.Runtime
{
    namespace ViewModel
    {
        public abstract class SnkViewModel : ObservableObject, ISnkViewModel
        {
            public virtual void ViewCreated() { }

            public virtual void ViewAppearing() { }

            public virtual void ViewAppeared() { }

            public virtual void ViewDisappearing() { }

            public virtual void ViewDisappeared() { }

            public virtual void ViewDestroy(bool viewFinishing = true) { }

            
            public virtual void Start() { }

            public void Init(ISnkBundle parameters) => InitFromBundle(parameters);

            public void ReloadState(ISnkBundle state) => ReloadFromBundle(state);

            public void SaveState(ISnkBundle state) => SaveStateToBundle(state);

            protected virtual void InitFromBundle(ISnkBundle parameters) { }

            protected virtual void ReloadFromBundle(ISnkBundle state) { }

            protected virtual void SaveStateToBundle(ISnkBundle bundle) { }

            public abstract void Prepare();

            public virtual Task Initialize() => Task.FromResult(true);

            private SnkNotifyTask _initializeTask;

            public SnkNotifyTask InitializeTask
            {
                get => _initializeTask;
                set => Set(ref this._initializeTask, value, nameof(_initializeTask));
            }
        }
    }
}