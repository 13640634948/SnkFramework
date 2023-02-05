namespace SnkFramework.Mvvm.Runtime.ViewModel
{
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
}