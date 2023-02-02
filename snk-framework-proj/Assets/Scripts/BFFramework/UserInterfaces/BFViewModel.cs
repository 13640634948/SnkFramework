using System.Threading.Tasks;
using Loxodon.Framework.Observables;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace BFFramework.Runtime.UserInterface
{
    public abstract class BFViewModel : ObservableObject, ISnkViewModel
    {
        public abstract void ViewCreated();
        public abstract void ViewAppearing();
        public abstract void ViewAppeared();
        public abstract void ViewDisappearing();
        public abstract void ViewDisappeared();
        public abstract void ViewDestroy(bool viewFinishing = true);
        public abstract void Init(ISnkBundle parameters);
        public abstract void ReloadState(ISnkBundle state);
        public abstract void Start();
        public abstract void SaveState(ISnkBundle state);
        public abstract void Prepare(ISnkBundle parameterBundle);
        public abstract Task Initialize();
        public abstract SnkNotifyTask InitializeTask { get; set; }
    }
}