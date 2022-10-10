using System.Collections.Generic;

namespace SampleDevelop.Test
{
    public interface ISnkUIContainer
    {
        public List<SnkUIPageBase> mPageList { get; }

        public SnkUIPageBase GetPage(string name);
        public TPage GetPage<TPage>(string name) where TPage : SnkUIPageBase;
        public void AddView(SnkUIPageBase page);
        public void RemoveView(SnkUIPageBase page);
    }
}