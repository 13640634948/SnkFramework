using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using SnkFramework.Mvvm.Runtime.View;

namespace BFFramework.Runtime.UserInterface
{
    public class BFViewsContainer : SnkViewsContainer
    {
        public override async Task<SnkWindow> CreateView(Type viewType)
        {
            var asset = await Resources.LoadAsync<GameObject>("UserInterfaces/Windows/" + viewType.Name);
            var inst = UnityEngine.Object.Instantiate(asset) as GameObject;
            if (inst == null)
                return null;
            inst.name = viewType.Name;
            return (inst.GetComponent<SnkWindow>() ?? inst.AddComponent(viewType)) as SnkWindow;
        }
    }
}