namespace SampleDevelop.Test
{
    public interface ISnkUIPage : ISnkUIView
    {
    }
    public interface ISnkUIPage<TViewOwner> : ISnkUIPage, ISnkUIView<TViewOwner>
        where TViewOwner : class, ISnkViewOwner
    {
    }
}