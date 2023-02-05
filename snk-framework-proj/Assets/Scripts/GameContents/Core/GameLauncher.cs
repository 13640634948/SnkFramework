using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SnkFramework.Runtime.Engine;
using UnityEngine;

namespace GAME.Contents.Core
{
    public class GameLauncher
    {
        private static string[] namespaces =
        {
            "SnkFramework.Runtime",
            "BFFramework.Runtime",
            "Game.Contexts.",
        };
        
        [RuntimeInitializeOnLoadMethod]
        public static void bootup()
        {
            var assemblies = GetGameAssembly();
            var instance = UnitySetupSingleton.EnsureSingletonAvailable<GameSetup>(assemblies.ToArray());
            instance.EnsureInitialized();
            instance.AsyncRunAppStart();
        }

        private static IEnumerable<Assembly> GetGameAssembly()
        {
            return from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from ns in namespaces
                where assembly.FullName.StartsWith(ns)
                select assembly;
        }
    }
}