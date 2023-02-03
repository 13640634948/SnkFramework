using System;
using BFFramework.Runtime.Core;
using BFFramework.Runtime.UserInterface;
using GAME.Contents.UserInterfaces.ViewModels;
using GAME.Contents.UserInterfaces.Views;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Runtime.Engine;
using UnityEngine;

namespace GAME.Contents.Core
{
    public class ViewLoader : BFViewLoader
    {
        public override Type GetViewType(Type viewModelType)
        {
            return typeof(SplashScreenWindow);
        }
    }

    public class GameLauncher
    {
        [RuntimeInitializeOnLoadMethod]
        public static void bootup()
        {
            var instance = UnitySetupSingleton.EnsureSingletonAvailable<Setup>(null);
            instance.EnsureInitialized();
            instance.AsyncRunAppStart();
        }
    }
    
    public class Setup : BFGameSetup<App>
    {
        protected override ISnkViewLoader CreateViewLoader() => new ViewLoader();
    }

    public class App : BFClientApp
    {
        public override void Initialize()
        {
            base.Initialize();
            RegisterAppStart<SplashScreenViewModel>();
        }
    }
}