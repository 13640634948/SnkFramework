namespace SampleDevelop.Test
{
    public interface ISnkUIView<TViewOwner> : ISnkView<TViewOwner>
        where TViewOwner : class, ISnkViewOwner
    {
        
    }

    public interface ISnkUIView : ISnkView
    {
        public bool mInteractable { get; set; }
    }
 
}