using SnkFramework.Runtime.Engine;
using UnityEngine;

namespace GAME.Contents.Core
{
    public class GameLauncher
    {
        [RuntimeInitializeOnLoadMethod]
        public static void bootup()
        {
            var instance = UnitySetupSingleton.EnsureSingletonAvailable<GameSetup>(null);
            instance.EnsureInitialized();
            instance.AsyncRunAppStart();
        }
    }
}