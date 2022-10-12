using System.Collections.Generic;

namespace SampleDevelop.Test
{
    public abstract class SnkWindowView : SnkUIPage, ISnkWindowView, ISnkUIContainer
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
        private List<SnkUIPage> _pageList;
        public virtual List<SnkUIPage> mPageList => _pageList ??= new List<SnkUIPage>();

        public virtual SnkUIPage GetPage(string name)
            => this._pageList.Find(a => a.mName.Equals(name));

        public virtual TPage GetPage<TPage>(string name) where TPage : SnkUIPage
            => this.GetPage(name) as TPage;

        public virtual void AddView(SnkUIPage page)
        {
            page.mParentView = this;
            this._pageList.Add(page);
        }

        public virtual void RemoveView(SnkUIPage page)
        {
            page.mParentView = null;
            this._pageList.Remove(page);
        }
    }
    
   

}