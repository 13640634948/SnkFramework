using System.Collections.Generic;

namespace SampleDevelop.Test
{
    public abstract class SnkWindowViewBase : SnkUIPageBase, ISnkWindowView, ISnkUIContainer
    {
        private ISnkAnimation _activationAnimation;
        private ISnkAnimation _passivationAnimation;
        
        public ISnkAnimation mActivationAnimation 
        { 
            get=> this._activationAnimation;
            set => this._activationAnimation = value;
        }
        
        public ISnkAnimation mPassivationAnimation
        { 
            get=> this._passivationAnimation;
            set => this._passivationAnimation = value;
        }
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