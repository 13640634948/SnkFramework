
namespace SampleDevelop.Test
{
    public abstract class SnkUIPage : SnkUIView, ISnkUIPage
    {
        public override void Create()
        {
            this.mVisibility = false;
        }
    }
}