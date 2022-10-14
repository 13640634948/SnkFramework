using System;
using System.Threading;
using SnkFramework.Mvvm;
using SnkFramework.Mvvm.Core;
using SnkFramework.Mvvm.Core.View;
using SnkFramework.Mvvm.Runtime.UGUI;
using UnityEngine;

namespace SampleDevelop.Mvvm
{
    public class SnkDemoMvvmSetup : SnkMvvmSetup
    {
        private SynchronizationContext _synchronizationContext;

        protected override void RegisterDefaultSetupDependencies(SnkIoCProvider iocProvider)
        {
            base.RegisterDefaultSetupDependencies(iocProvider);
            iocProvider.Register<ISnkMvvmSettings>(new UGUIMvvmSetting());
        }

        public void PlatformInitialize(SynchronizationContext synchronizationContext)
        {
            if (synchronizationContext == null)
                throw new ArgumentNullException(nameof(synchronizationContext));

            _synchronizationContext = synchronizationContext;
        }

        protected override void InitializeMvvmManager(SnkIoCProvider iocProvider)
        {
            _synchronizationContext.Send(state =>
            {
                UGUISnkMvvmManager mvvmMgr = new UGUISnkMvvmManager();
                iocProvider.Register<ISnkMvvmManager>(mvvmMgr);
            }, null);
        }

        protected override void InitializeMvvmCoroutineExecutor(SnkIoCProvider iocProvider)
        {
            _synchronizationContext.Send(state =>
            {
                GameObject gameObject = new GameObject(nameof(DemoCoroutineExecutor));
                DemoCoroutineExecutor executor = gameObject.AddComponent<DemoCoroutineExecutor>();
                iocProvider.Register<ISnkMvvmCoroutineExecutor>(executor);
            }, null);
        }

        protected override void InitializeTransitionExecutor(SnkIoCProvider iocProvider)
        {
            _synchronizationContext.Send(state =>
            {
                var coroutineExecutor = iocProvider.Resolve<ISnkMvvmCoroutineExecutor>();
                SnkTransitionPopupExecutor executor = new SnkTransitionPopupExecutor(coroutineExecutor);
                iocProvider.Register<ISnkTransitionExecutor>(executor);
            }, null);
        }

        protected override void InitializeMvvmLoader(SnkIoCProvider iocProvider)
        {
            _synchronizationContext.Send(state => { iocProvider.Register<ISnkMvvmLoader>(new SnkMvvmLoader()); }, null);
        }
    }
}