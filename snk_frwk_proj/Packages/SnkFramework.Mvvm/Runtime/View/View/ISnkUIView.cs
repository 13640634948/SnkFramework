using System.Collections;
using Loxodon.Framework.Binding.Contexts;

namespace SampleDevelop.Test
{
    public interface ISnkUIView : ISnkView
    {
        public bool mInteractable { get; set; }
    }
 
}