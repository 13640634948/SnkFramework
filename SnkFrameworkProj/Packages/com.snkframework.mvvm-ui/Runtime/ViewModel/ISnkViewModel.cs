using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Base;

namespace SnkFramework.Mvvm.Runtime
{
    namespace ViewModel
    {
        public interface ISnkViewModel
        {
            void ViewCreated();

            void ViewAppearing();

            void ViewAppeared();

            void ViewDisappearing();

            void ViewDisappeared();

            void ViewDestroy(bool viewFinishing = true);

            void Init(ISnkBundle parameters);

            void ReloadState(ISnkBundle state);

            void Start();

            void SaveState(ISnkBundle state);

            void Prepare(ISnkBundle parameterBundle);

            Task Initialize();

            SnkNotifyTask InitializeTask { get; set; }
        }
        
    }
}