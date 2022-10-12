using SnkFramework.Mvvm.ViewModel;

namespace SampleDevelop.Test
{
    public interface ISnkUIView<TViewOwner, TViewModel> : ISnkView<TViewOwner, TViewModel>
        where TViewOwner : class, ISnkViewOwner
        where TViewModel : class, ISnkViewModel
    {
        
    }

    public interface ISnkUIView : ISnkView
    {
        public bool mInteractable { get; set; }
    }
 
}