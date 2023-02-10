using System.Threading.Tasks;
using BFFramework.Runtime.Core;
using BFFramework.Runtime.Services;
using Cysharp.Threading.Tasks;
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
            //RegisterCustomAppStart<GameAppStart>();
        }

        protected override void RegisterServices()
        {
            base.RegisterServices();
            this.RegisterService<IAuthenticationService, AuthenticationService>();
        }

        public override async Task Startup()
        {
            Game.Resolve<IBFPatchService>().Initialize();
            await Game.ResolveService<IBFSceneService>().LoadSceneAsync("LoginScene", true);
        }
    }
}