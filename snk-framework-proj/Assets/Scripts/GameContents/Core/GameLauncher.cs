using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BFFramework.Runtime.Services;
using Cysharp.Threading.Tasks;
using GAME.Contents.UserInterfaces;
using SnkFramework.Runtime.Core;
using SnkFramework.Runtime.Engine;
using UnityEngine;

namespace GAME.Contents
{
    namespace Core
    {
        public class GameLauncher : MonoBehaviour
        {
            public string[] RootNamespaces;

            public SplashScreen SplashScreenAsset;

            private IEnumerable<Assembly> GetGameAssembly()
            {
                return from assembly in AppDomain.CurrentDomain.GetAssemblies()
                    from ns in RootNamespaces
                    where assembly.FullName.StartsWith(ns)
                    select assembly;
            }

            public async void Start()
            {
                var splashScreen = Instantiate(SplashScreenAsset).GetComponent<SplashScreen>();
                splashScreen.FadeOutDuration = 1.0f;
                splashScreen.Play();
            
                await UniTask.WaitUntil(() => splashScreen.mPrepareCompleted);
                var assemblies = GetGameAssembly().ToArray();
                var instance = await SnkUnitySetupSingleton.EnsureSingletonAvailable<GameSetup>(assemblies);
                await UniTask.WaitUntil(() => splashScreen.mPrepareCompleted);
                await instance.EnsureInitialized(null);

                //闪屏中显示第一个界面&加载登陆场景&初始化热更
                await Game.ResolveService<ISnkAppStart>()?.StartAsync();
            
                await UniTask.WaitUntil(() => splashScreen.mFinish);//屏蔽这行，提前结束闪屏

                await Game.ResolveService<IBFSceneService>().UnloadCurrScene();
            }
        }
    }
  
}