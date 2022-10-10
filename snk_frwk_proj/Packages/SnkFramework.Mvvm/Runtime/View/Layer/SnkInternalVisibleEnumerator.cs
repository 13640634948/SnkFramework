using System.Collections;
using System.Collections.Generic;

namespace SampleDevelop.Test
{
    internal class SnkInternalVisibleEnumerator : IEnumerator<ISnkWindow>
    {
        private List<ISnkWindow> windows;
        private int index = -1;
        public SnkInternalVisibleEnumerator(List<ISnkWindow> list)
        {
            this.windows = list;
        }

        public ISnkWindow Current
        {
            get { return this.index < 0 || this.index >= this.windows.Count ? null : this.windows[index]; }
        }

        object IEnumerator.Current
        {
            get { return this.Current; }
        }

        public void Dispose()
        {
            this.index = -1;
            this.windows.Clear();
        }

        public bool MoveNext()
        {
            if (index >= this.windows.Count - 1)
                return false;

            index++;
            for (; index < this.windows.Count; index++)
            {
                ISnkWindow window = this.windows[index];
                if (window != null && window.mVisibility)
                    return true;
            }

            return false;
        }

        public void Reset()
        {
            this.index = -1;
        }
    }
}