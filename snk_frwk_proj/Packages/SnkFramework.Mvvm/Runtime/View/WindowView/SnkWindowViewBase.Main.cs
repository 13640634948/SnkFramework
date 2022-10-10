namespace SampleDevelop.Test
{
    public  abstract partial class SnkWindowViewBase : SnkUIPageBase, ISnkWindowView
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
    }
    
   

}