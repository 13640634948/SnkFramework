using SnkFramework.Mvvm.ViewModel;

namespace SampleDevelop.Test
{
    public interface ISnkUIPage : ISnkUIView
    {
    }
    public interface ISnkUIPage<TViewOwner, TViewModel> : ISnkUIPage, ISnkUIView<TViewOwner, TViewModel>
        where TViewOwner : class, ISnkViewOwner
        where TViewModel : class, ISnkViewModel
    {
    }
}