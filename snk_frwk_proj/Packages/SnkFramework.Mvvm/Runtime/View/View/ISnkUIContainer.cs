using System.Collections.Generic;

namespace SnkFramework.Mvvm.Core
{
    namespace View
    {
        public interface ISnkUIContainer
        {
            public List<SnkUIPage> mPageList { get; }

            public SnkUIPage GetPage(string name);
            public TPage GetPage<TPage>(string name) where TPage : SnkUIPage;
            public void AddView(SnkUIPage page);
            public void RemoveView(SnkUIPage page);
        }
    }
}