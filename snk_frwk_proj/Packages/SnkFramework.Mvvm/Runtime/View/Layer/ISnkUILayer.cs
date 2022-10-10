using System.Collections.Generic;
namespace SampleDevelop.Test
{
    public interface ISnkUILayer : ISnkTransitionController
    {
         bool Activated { get; set; }

        ISnkWindow Current { get; }

        int Count { get; }

        IEnumerator<ISnkWindow> Visibles();

        ISnkWindow Get(int index);

        void Add(ISnkWindow window);

        bool Remove(ISnkWindow window);

        ISnkWindow RemoveAt(int index);

        bool Contains(ISnkWindow window);

        int IndexOf(ISnkWindow window);

        List<ISnkWindow> Find(bool visible);

        T Find<T>() where T : class, ISnkWindow;

        T Find<T>(string name) where T : class, ISnkWindow;

        List<T> FindAll<T>() where T : class, ISnkWindow;

        void Clear();

        public int GetPrioritySerialNum();
    }
}