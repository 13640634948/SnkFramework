using System.Collections.Generic;

namespace SampleDevelop.Test
{
    public partial class SnkWindowViewBase : ISnkUIContainer
    {
        private List<SnkUIPageBase> _pageList;
        public virtual List<SnkUIPageBase> mPageList => _pageList ??= new List<SnkUIPageBase>();

        public virtual SnkUIPageBase GetPage(string name)
            => this._pageList.Find(a => a.mName.Equals(name));

        public virtual TPage GetPage<TPage>(string name) where TPage : SnkUIPageBase
            => this.GetPage(name) as TPage;

        public virtual void AddView(SnkUIPageBase page)
        {
            page.mParentView = this;
            this._pageList.Add(page);
        }

        public virtual void RemoveView(SnkUIPageBase page)
        {
            page.mParentView = null;
            this._pageList.Remove(page);
        }
    }
}