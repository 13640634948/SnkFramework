using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SnkFramework.Runtime.Core;
using SnkFramework.Runtime.Engine;
using BFFramework.Runtime.Services;
using UnityEngine;
using Cysharp.Threading.Tasks;
using SnkFramework.Runtime;

namespace GAME.Contents
{
    namespace Core
    {
        public class GameLauncher : MonoBehaviour
        {
            [Header("跳过闪屏")]
            public bool splashSkip = true;
            
            [Header("闪屏淡出耗时")]
            public float splashFadeOutDuration = 0.5f;
            
            [Header("闪屏视图对象资源")]
            public SplashScreen splashScreenAsset;

            [Header("核心命名空间(程序集)")]
            public string[] rootNamespaces;

            private IEnumerable<Assembly> GetGameAssembly()
            {
                return from assembly in AppDomain.CurrentDomain.GetAssemblies()
                    from ns in rootNamespaces
                    where assembly.FullName.StartsWith(ns)
                    select assembly;
            }

            public async void Start()
            { 
                var splashScreen = Instantiate(splashScreenAsset).GetComponent<SplashScreen>();
                splashScreen.SetFadeOutDuration(splashFadeOutDuration);
                splashScreen.Play();
            
                await UniTask.WaitUntil(() => splashScreen.mPrepareCompleted);
                var assemblies = GetGameAssembly().ToArray();
                var instance = await SnkUnitySetupSingleton.EnsureSingletonAvailable<GameSetup>(assemblies);
                await UniTask.WaitUntil(() => splashScreen.mPrepareCompleted);
                await instance.EnsureInitialized(null);

                //闪屏中显示第一个界面&加载登陆场景&初始化热更
                await Snk.IoCProvider.Resolve<ISnkAppStart>()?.StartAsync();
            
                if(splashSkip)
                    splashScreen.FadeOut();
                
                await UniTask.WaitUntil(() => splashScreen.mFinish);
                await Snk.IoCProvider.Resolve<IBFSceneService>().UnloadCurrScene();
            }
        }
    }
}
