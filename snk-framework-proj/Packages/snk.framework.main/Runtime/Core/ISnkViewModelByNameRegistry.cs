using System;
using System.Reflection;
using SnkFramework.Mvvm.Runtime.ViewModel;

namespace SnkFramework.Runtime.Core
{
    public interface IMvxNameMapping
    {
        string Map(string inputName);
    }

    public class MvxViewToViewModelNameMapping
        : IMvxNameMapping
    {
        public string ViewModelPostfix { get; set; }

        public MvxViewToViewModelNameMapping()
        {
            ViewModelPostfix = "ViewModel";
        }

        public virtual string Map(string inputName)
        {
            return inputName + ViewModelPostfix;
        }
    }
    
    public class MvxPostfixAwareViewToViewModelNameMapping
        : MvxViewToViewModelNameMapping
    {
        private readonly string[] _postfixes;

        public MvxPostfixAwareViewToViewModelNameMapping(params string[] postfixes)
        {
            _postfixes = postfixes;
        }

        public override string Map(string inputName)
        {
            foreach (var postfix in _postfixes)
            {
                if (inputName.EndsWith(postfix) && inputName.Length > postfix.Length)
                {
                    inputName = inputName.Substring(0, inputName.Length - postfix.Length);
                    break;
                }
            }
            return base.Map(inputName);
        }
    }
    public interface IMvxTypeFinder
    {
        Type? FindTypeOrNull(Type candidateType);
    }
    public interface IMvxViewModelTypeFinder
        : IMvxTypeFinder
    {
    }

    public interface ISnkViewModelByNameRegistry
    {
        void Add(Type viewModelType);

        void Add<TViewModel>() where TViewModel : ISnkViewModel;

        void AddAll(Assembly assembly);
    }
}