using BFFramework.Runtime.Core;
using GAME.Contents.Services;
using GAME.Contents.UserInterfaces.ViewModels;

namespace GAME.Contents.Core
{
    public class GameApp : BFClientApp
    {
        public override void Initialize()
        {
            base.Initialize();
            
            RegisterAppStart<SplashScreenViewModel>();
        }

        protected override void InitializeService()
        {
            base.InitializeService();
            this.RegisterService<IAuthenticationService, AuthenticationService>();
        }
    }
}