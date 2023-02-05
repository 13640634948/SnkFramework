using SnkFramework.Mvvm.Runtime;
using SnkFramework.Runtime.Core;

namespace GAME.Contents.UserInterfaces.ViewModels
{
    [SnkAppStart]
    public class AppStart : SnkAppStart<SplashScreenViewModel>
    {
        public AppStart(ISnkApplication application, ISnkMvvmService navigationService) : base(application, navigationService)
        {
        }
    }
}