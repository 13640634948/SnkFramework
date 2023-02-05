using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Presenters;
using SnkFramework.Mvvm.Runtime.ViewModel;
using UnityEngine;

namespace SnkFramework.Mvvm.Runtime
{
    namespace View
    {
        public interface IMvxViewsContainer : ISnkViewFinder
        {
            void AddAll(IDictionary<Type, Type> viewModelViewLookup);

            void Add(Type viewModelType, Type viewType);

            void Add<TViewModel, TView>()
                where TViewModel : ISnkViewModel
                where TView : ISnkView;

            void AddSecondary(ISnkViewFinder finder);

            void SetLastResort(ISnkViewFinder finder);
        }
        
        public abstract class SnkViewLoader : IMvxViewsContainer, ISnkViewLoader
        {
            private readonly Dictionary<Type, Type> _bindingMap = new Dictionary<Type, Type>();
            private readonly List<ISnkViewFinder> _secondaryViewFinders;
            private ISnkViewFinder? _lastResortViewFinder;
            
            public void Add(Type viewModelType, Type viewType)
            {
                Debug.Log("SnkViewLoader-Add:" + viewModelType + ", " + viewType);
                _bindingMap[viewModelType] = viewType;
            }

            public void Add<TViewModel, TView>()
                where TViewModel : ISnkViewModel
                where TView : ISnkView
            {
                Add(typeof(TViewModel), typeof(TView));
            }

            public void AddSecondary(ISnkViewFinder finder)
            {
                _secondaryViewFinders.Add(finder);
            }

            public void SetLastResort(ISnkViewFinder finder)
            {
                _lastResortViewFinder = finder;
            }

            public void AddAll(IDictionary<Type, Type> viewModelViewLookup)
            {
                foreach (var pair in viewModelViewLookup)
                {
                    Add(pair.Key, pair.Value);
                }
            }

            public virtual async Task<SnkWindow> CreateView(SnkViewModelRequest request)
            {
                var viewType = GetViewType(request.ViewModelType);
                Debug.Log("CreateView-viewType:" + viewType);
                return await CreateView(viewType);
            }

            public abstract Task<SnkWindow> CreateView(Type viewType);

            public virtual bool UnloadView(SnkWindow window)
            {
                UnityEngine.Object.Destroy(window.gameObject);
                return true;
            }

            public virtual Type GetViewType(Type viewModelType)
            {
                Type? binding;
                if (viewModelType != null && _bindingMap.TryGetValue(viewModelType, out binding))
                {
                    return binding;
                }

                foreach (var viewFinder in _secondaryViewFinders)
                {
                    binding = viewFinder.GetViewType(viewModelType);
                    if (binding != null)
                    {
                        return binding;
                    }
                }

                if (_lastResortViewFinder != null)
                {
                    binding = _lastResortViewFinder.GetViewType(viewModelType);
                    if (binding != null)
                    {
                        return binding;
                    }
                }

                throw new KeyNotFoundException("Could not find view for " + viewModelType);
            }

            public virtual Type GetViewType<TViewModel>() where TViewModel : class, ISnkViewModel
                => GetViewType(typeof(TViewModel));
        }
    }
}