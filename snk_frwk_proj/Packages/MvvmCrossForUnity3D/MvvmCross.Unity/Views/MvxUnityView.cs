using MvvmCross.Binding.BindingContext;
using MvvmCross.ViewModels;

namespace MvvmCross.Unity.Views
{
    public interface IMvxUnityObject
    {
        public UnityEngine.Object UnityObject { get; set; }
    }

    public interface IMvxUnityObject<TUnityObject> 
        where TUnityObject : UnityEngine.Object
    {
        public new TUnityObject UnityObject { get; set; }
    }

    public abstract class MvxUnityView :  IMvxUnityView, IMvxUnityObject
    {
        public UnityEngine.Object UnityObject { get; set; }

        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }

        public IMvxViewModel ViewModel
        {
            get { return DataContext as IMvxViewModel; }
            set { DataContext = value; }
        }

        public IMvxBindingContext BindingContext { get; set; }
    }


    public abstract class MvxUnityView<TUnityObject> : MvxUnityView, IMvxUnityObject<TUnityObject>
        where TUnityObject : UnityEngine.Object
    {
        public new TUnityObject UnityObject
        {
            get => base.UnityObject as TUnityObject;
            set => base.UnityObject = value;
        }
    }


    public class MvxUnityView<TViewModel, TUnityObject> : MvxUnityView<TUnityObject> , IMvxUnityView<TViewModel>
        where TViewModel : class, IMvxViewModel
        where TUnityObject : UnityEngine.Object
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public MvxFluentBindingDescriptionSet<IMvxUnityView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<IMvxUnityView<TViewModel>, TViewModel>();
        }
    }
}