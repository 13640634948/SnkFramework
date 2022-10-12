using System.Collections;

namespace SnkFramework.Mvvm.Base
{
    public interface IMvvmCoroutineExecutor
    {
        public void RunOnCoroutineNoReturn(IEnumerator routine);
    }
}