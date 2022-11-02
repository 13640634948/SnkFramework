using System.Threading.Tasks;
using Loxodon.Framework.Observables;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.ViewModel;

public class TestViewModel : ObservableObject, ISnkViewModel
{
    public void ViewCreated()
    {
        //throw new System.NotImplementedException();
    }

    public void ViewAppearing()
    {
        //throw new System.NotImplementedException();
    }

    public void ViewAppeared()
    {
        //throw new System.NotImplementedException();
    }

    public void ViewDisappearing()
    {
        //throw new System.NotImplementedException();
    }

    public void ViewDisappeared()
    {
        //throw new System.NotImplementedException();
    }

    public void ViewDestroy(bool viewFinishing = true)
    {
        //throw new System.NotImplementedException();
    }

    public void Init(ISnkBundle parameters)
    {
        //throw new System.NotImplementedException();
    }

    public void ReloadState(ISnkBundle state)
    {
        //throw new System.NotImplementedException();
    }

    public void Start()
    {
        //throw new System.NotImplementedException();
    }

    public void SaveState(ISnkBundle state)
    {
        //throw new System.NotImplementedException();
    }

    public void Prepare(ISnkBundle parameterBundle)
    {
        //throw new System.NotImplementedException();
    }

    public void Prepare()
    {
        //throw new System.NotImplementedException();
    }

    public Task Initialize()
    {
        return Task.FromResult(true);
    }

    private SnkNotifyTask _initializeTask;
    public SnkNotifyTask InitializeTask
    {
        get => _initializeTask;
        set => this.Set(ref this._initializeTask, value); 

    }
}