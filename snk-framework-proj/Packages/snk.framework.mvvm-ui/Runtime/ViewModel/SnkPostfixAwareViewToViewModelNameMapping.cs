namespace SnkFramework.Mvvm.Runtime
{
    namespace ViewModel
    {
        public class SnkPostfixAwareViewToViewModelNameMapping
            : SnkViewToViewModelNameMapping
        {
            private readonly string[] _postfixes;

            public SnkPostfixAwareViewToViewModelNameMapping(params string[] postfixes)
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
    }
}