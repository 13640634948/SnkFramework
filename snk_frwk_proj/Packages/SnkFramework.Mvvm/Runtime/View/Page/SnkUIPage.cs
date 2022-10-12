
namespace SnkFramework.Mvvm.View
{
    public abstract partial class SnkUIPage : SnkUIView, ISnkUIPage
    {
        public override void Create()
        {
            this.mVisibility = false;
        }
    }
}