using System.Threading.Tasks;
using BFFramework.Runtime.Core;
using Cysharp.Threading.Tasks;
using GAME.Contents.Services;
using GAME.Contents.UserInterfaces.ViewModels;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

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

        protected override void InitializeService()
        {
            base.InitializeService();
            this.RegisterService<IAuthenticationService, AuthenticationService>();
        }

        public override async Task Startup()
        {
            await EditorSceneManager.LoadSceneAsync("LoginScene", LoadSceneMode.Additive);
            //return base.Startup();
        }
    }
}