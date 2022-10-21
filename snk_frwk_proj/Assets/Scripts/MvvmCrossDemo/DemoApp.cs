using MvvmCross.ViewModels;

namespace MvvmCross.Demo
{
    public class DemoApp : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();
            RegisterAppStart<DemoViewModel, DemoStartupViewModelParameter>();
        }
    }
}