
namespace SnkFramework.Mvvm.Core
{
    namespace View
    {
        public abstract partial class SnkUIPage : SnkUIView, ISnkUIPage
        {
            public override void Create()
            {
                this.mVisibility = false;
            }
        }
    }
}