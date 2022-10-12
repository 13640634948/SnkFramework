using System;
using System.Collections;
using SnkFramework.Mvvm.View;

namespace SnkFramework.Mvvm.Base
{
    public interface IMvvmLoader
    {
        public ISnkViewOwner LoadViewOwner(string ownerPath);
        public IEnumerator LoadViewOwnerAsync(string ownerPath, Action<ISnkViewOwner> callback);
    }
}