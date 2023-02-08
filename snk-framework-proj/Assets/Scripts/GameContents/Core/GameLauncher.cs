using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SnkFramework.Runtime.Core;
using SnkFramework.Runtime.Engine;
using UnityEngine;

namespace GAME.Contents.Core
{
    public class GameLauncher : MonoBehaviour, ISnkSetupMonitor
    {
        private static string[] namespaces =
        {
            "SnkFramework.Runtime",
            "BFFramework.Runtime",
            "Game.Contexts.",
        };
        
        public async void Start()
        {
            var assemblies = GetGameAssembly();
            var instance = await SnkUnitySetupSingleton.EnsureSingletonAvailable<GameSetup>(assemblies.ToArray());
            await instance.EnsureInitialized(this);
            instance.AsyncRunAppStart();
        }

        private static IEnumerable<Assembly> GetGameAssembly()
        {
            return from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from ns in namespaces
                where assembly.FullName.StartsWith(ns)
                select assembly;
        }

        public async Task InitializationComplete()
        {
            //await UniTask.WaitUntil(() => finish);
        }
    }
}