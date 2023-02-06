using BFFramework.Runtime.Core;
using GAME.Contents.Services;

namespace GAME.Contents.Core
{
    public class GameApp : BFClientApp
    {
        public override void Initialize()
        {
            base.Initialize();
            this.RegisterService<IAuthenticationService, AuthenticationService>();
        }
    }
}