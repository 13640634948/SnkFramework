using System.Collections;

namespace SnkFramework.Mvvm.Core
{
    public interface ISnkMvvmCoroutineExecutor
    {
        public void RunOnCoroutineNoReturn(IEnumerator routine);
    }
}