using System;
using System.Runtime.CompilerServices;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Mvvm.Runtime.ViewModel;
using UnityEngine;

namespace SnkFramework.Mvvm.Runtime
{
    namespace View
    {
        public interface ISnkAwaiter : INotifyCompletion
        {
            public bool IsCompleted { get; }
            public void GetResult();
        }

        public interface ISnkAwaiter<T> : INotifyCompletion
        {
            public bool IsCompleted { get; }
            public T GetResult();
        }

        public class SnkTransitionOperation
        {
            public bool IsDone = false;

            public Action onCompleted;

            public ISnkAwaiter GetAwaiter()
                => new SnkTransitionOperationAwaiter(this);
        }

        public struct SnkTransitionOperationAwaiter : ISnkAwaiter, ICriticalNotifyCompletion
        {
            private SnkTransitionOperation _transitionOperation;
            private Action<SnkTransitionOperation> continuationAction;

            public SnkTransitionOperationAwaiter(SnkTransitionOperation asyncOperation)
            {
                this._transitionOperation = asyncOperation;
                this.continuationAction = null;
            }

            public bool IsCompleted => _transitionOperation.IsDone;

            public void GetResult()
            {
                Debug.Log("SnkTransitionOperation.GetResult:" + IsCompleted);
                //if (!IsCompleted)
                //    throw new Exception("The task is not finished yet");
            }

            public void OnCompleted(Action continuation)
            {
                UnsafeOnCompleted(continuation);
            }

            public void UnsafeOnCompleted(Action continuation)
            {
                this._transitionOperation.onCompleted = continuation;
            }
        }

        public struct SnkTransitionOperationAwaiter<TResult> : ISnkAwaiter<TResult>, ICriticalNotifyCompletion
        {
            private AsyncOperation asyncOperation;
            private Func<AsyncOperation, TResult> getter;
            private Action<AsyncOperation> continuationAction;

            public SnkTransitionOperationAwaiter(AsyncOperation asyncOperation, Func<AsyncOperation, TResult> getter)
            {
                this.asyncOperation = asyncOperation ?? throw new ArgumentNullException("asyncOperation");
                Debug.Log("asyncOperation:" + asyncOperation);
                this.getter = getter ?? throw new ArgumentNullException("getter");
                this.continuationAction = null;
            }

            public bool IsCompleted
            {
                get
                {
                    Debug.Log("IsUnithThread:" + UnitySyncContext.IsUnityThread);
                    return asyncOperation.isDone;
                }
            }

            public TResult GetResult()
            {
                if (!IsCompleted)
                    throw new Exception("The task is not finished yet");

                if (continuationAction != null)
                {
                    asyncOperation.completed -= continuationAction;
                    continuationAction = null;
                }

                return getter(asyncOperation);
            }

            public void OnCompleted(Action continuation)
            {
                UnsafeOnCompleted(continuation);
            }

            public void UnsafeOnCompleted(Action continuation)
            {
                if (continuation == null)
                    throw new ArgumentNullException("continuation");

                if (asyncOperation.isDone)
                {
                    continuation();
                }
                else
                {
                    continuationAction = (ao) => { continuation(); };
                    asyncOperation.completed += continuationAction;
                }
            }
        }


        public interface ISnkView
        {
            public ISnkViewModel ViewModel { get; set; }

            /// <summary>
            /// 视图是否显示
            /// </summary>
            public bool Visibility { get; set; }

            /// <summary>
            /// 视图是否可交互
            /// </summary>
            public bool Interactable { get; set; }

            /// <summary>
            /// 视图是否激活
            /// </summary>
            public bool Activated { get; set; }

            public void Create(ISnkBundle bundle);

            public SnkTransitionOperation Activate();

            public SnkTransitionOperation Passivate();
        }

        public interface ISnkNavigator
        {
            public ISnkView Current { get; }
            public ISnkView NavigatorPrev { get; }
            public ISnkView NavigatorNext { get; }
        }

        public interface ISnkPage : ISnkView, ISnkNavigator, ISnkContainer<ISnkView>
        {
            public ISnkWindow ParentWindow { get; }
        }

        public interface ISnkWindow : ISnkView, ISnkNavigator, ISnkContainer<ISnkPage>
        {
            public SnkWindowState WindowState { get; }
            public ISnkLayer Layer { get; }
            public SnkTransitionOperation Show(bool animated);
            public SnkTransitionOperation Hide(bool animated);

            public void AddPage(ISnkPage page);
            public TViewModel AddPage<TViewModel>();
            public SnkTransitionOperation AddPageAsync<TViewModel>();
        }

        public interface ISnkLayer
        {
            public string LayerName { get; }

            public void AddWindow(ISnkWindow window);
            public void RemoveWindow(ISnkWindow window);

            public SnkTransitionOperation ShowTransition(ISnkWindow window);
            public SnkTransitionOperation HideTransition(ISnkWindow window);
            public SnkTransitionOperation DismissTransition(ISnkWindow window);
        }
    }
}