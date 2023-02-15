using System.Threading.Tasks;
using BFFramework.Runtime.Core;
using BFFramework.Runtime.Services;
using GAME.Contents.Services;
using GAME.Contents.UserInterfaces.ViewModels;
using SnkFramework.Runtime;
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
            Snk.IoCProvider.Resolve<IBFPatchService>().Initialize();
            await Snk.IoCProvider.Resolve<IBFSceneService>().LoadSceneAsync("LoginScene", true);
        }
    }
}