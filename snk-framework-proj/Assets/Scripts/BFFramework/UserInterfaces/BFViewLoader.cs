using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using SnkFramework.Mvvm.Runtime.View;

namespace BFFramework.Runtime.UserInterface
{
    public class BFViewLoader : SnkViewLoader
    {
        public override async Task<SnkWindow> CreateView(Type viewType)
        {
            var asset = await Resources.LoadAsync<GameObject>(viewType.Name);
            var inst = UnityEngine.Object.Instantiate(asset) as GameObject;
            if (inst == null)
                return null;
            inst.name = viewType.Name;
            return inst.AddComponent(viewType) as SnkWindow;
        }
    }
}