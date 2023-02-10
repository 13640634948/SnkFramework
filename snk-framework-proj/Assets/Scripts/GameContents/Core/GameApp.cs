using System.Threading.Tasks;
using BFFramework.Runtime.Core;
using BFFramework.Runtime.Services;
using GAME.Contents.Services;
using GAME.Contents.UserInterfaces.ViewModels;
using UnityEngine;

namespace GAME.Contents.Core
{
    public class GameApp : BFClientApp
    { 
        public override void Initialize()
        {
            base.Initialize();
            
            RegisterAppStart<PatchViewModel>();
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