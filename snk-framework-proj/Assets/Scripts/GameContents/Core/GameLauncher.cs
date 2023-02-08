using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GAME.Contents.UserInterfaces;
using SnkFramework.Runtime;
using SnkFramework.Runtime.Core;
using SnkFramework.Runtime.Engine;
using UnityEngine;

namespace GAME.Contents.Core
{
    public class GameLauncher : MonoBehaviour, ISnkSetupMonitor
    {
        private SplashScreen _splashScreen;
        
        private static string[] namespaces =
        {
            "SnkFramework.Runtime",
            "BFFramework.Runtime",
            "Game.Contexts.",
        };

        private static IEnumerable<Assembly> GetGameAssembly()
        {
            return from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from ns in namespaces
                where assembly.FullName.StartsWith(ns)
                select assembly;
        }

        private async Task<SplashScreen> LoadSplashScreen()
        {
            var asset = await Resources.LoadAsync<GameObject>("SplashScreen/SplashScreen") as GameObject;
            return Instantiate(asset).GetComponent<SplashScreen>();
        }

        public async void Start()
        {
            this._splashScreen = await LoadSplashScreen();
            this._splashScreen.Play();
            
            await UniTask.WaitUntil(() => this._splashScreen.mPrepareCompleted);
            var assemblies = GetGameAssembly().ToArray();
            var instance = await SnkUnitySetupSingleton.EnsureSingletonAvailable<GameSetup>(assemblies);
            await UniTask.WaitUntil(() => this._splashScreen.mPrepareCompleted);
            await instance.EnsureInitialized(this);
        }

        public async Task InitializationComplete()
        {
            await Snk.IoCProvider.Resolve<ISnkAppStart>()?.StartAsync();
            /** 这行代码可以考虑在编辑器中屏蔽，就可以跳过闪屏界面 */
            await UniTask.WaitUntil(() => this._splashScreen.mFinish);
            _splashScreen.Dispose(1.5f);
            Destroy(this.gameObject);
        }
    }
}