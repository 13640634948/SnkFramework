using SnkFramework.Runtime.Core;
using SnkFramework.Runtime.Engine;

namespace DefaultNamespace
{
    public class DemoApp : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();
            RegisterAppStart<SplashScreenViewModel>();
        }
    }
}