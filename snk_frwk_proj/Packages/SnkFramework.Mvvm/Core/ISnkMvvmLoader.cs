using System;
using System.Collections;
using SnkFramework.Mvvm.Core.View;

namespace SnkFramework.Mvvm.Core
{
    public interface ISnkMvvmLoader
    {
        public ISnkViewOwner LoadViewOwner(string ownerPath);
        public IEnumerator LoadViewOwnerAsync(string ownerPath, Action<ISnkViewOwner> callback);
    }
}