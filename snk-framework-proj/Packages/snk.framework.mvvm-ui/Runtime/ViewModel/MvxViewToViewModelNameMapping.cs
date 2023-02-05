namespace SnkFramework.Mvvm.Runtime.ViewModel
{
    public class MvxViewToViewModelNameMapping : IMvxNameMapping
    {
        public string ViewModelPostfix { get; set; }

        public MvxViewToViewModelNameMapping()
        {
            ViewModelPostfix = "ViewModel";
        }

        public virtual string Map(string inputName)
        {
            return inputName + ViewModelPostfix;
        }
    }
}