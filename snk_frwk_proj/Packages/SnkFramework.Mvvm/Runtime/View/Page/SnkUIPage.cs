
namespace SnkFramework.Mvvm.View
{
    public abstract class SnkUIPage : SnkUIView, ISnkUIPage
    {
        public override void Create()
        {
            this.mVisibility = false;
        }
    }
}