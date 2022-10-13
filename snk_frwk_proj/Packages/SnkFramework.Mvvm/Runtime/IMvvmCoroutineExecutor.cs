using System.Collections;

namespace SnkFramework.Mvvm.Core
{
    public interface IMvvmCoroutineExecutor
    {
        public void RunOnCoroutineNoReturn(IEnumerator routine);
    }
}