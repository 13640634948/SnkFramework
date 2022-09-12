using SnkFramework.Mvvm.View;

namespace SnkFramework.Mvvm.Base
{
    public interface ILoadOperation
    {
        public IWindow mTarget { get; }
        public float mProgress { get; }
        public bool mIsDone { get; }
    }
}