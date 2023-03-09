namespace SnkFramework.Mvvm.Runtime
{
    namespace ViewModel
    {
        public class SnkViewToViewModelNameMapping : ISnkNameMapping
        {
            public string ViewModelPostfix { get; set; }

            public SnkViewToViewModelNameMapping()
            {
                ViewModelPostfix = "ViewModel";
            }

            public virtual string Map(string inputName)
            {
                return inputName + ViewModelPostfix;
            }
        }
    }
}