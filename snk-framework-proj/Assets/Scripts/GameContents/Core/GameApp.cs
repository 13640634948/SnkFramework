using BFFramework.Runtime.Core;
using GAME.Contents.Services;

namespace GAME.Contents.Core
{
    public class GameApp : BFClientApp
    {
        public override void Initialize()
        {
            base.Initialize();
            
            //RegisterAppStart<SplashScreenViewModel>();
            RegisterCustomAppStart<GameAppStart>();
        }

        protected override void InitializeService()
        {
            base.InitializeService();
            this.RegisterService<IAuthenticationService, AuthenticationService>();
        }
    }
}