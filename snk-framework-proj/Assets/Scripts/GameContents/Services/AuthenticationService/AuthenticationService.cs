using System.Threading;

namespace GAME.Contents.Services
{
    /// <summary>
    /// 鉴权服务：黑名单or白名单，编辑器or运行时，内网or外网等
    /// </summary>
    public class AuthenticationService : GameServiceBase, IAuthenticationService
    {
        public AuthenticationService()
        {
            Thread.Sleep(2000);
        }
    }
}