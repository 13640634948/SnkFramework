using System.Collections.Generic;
using SnkFramework.Network.FileStorage.Runtime;

namespace SnkFramework.Network.FileStorage
{
    namespace Editor
    {
        public interface ISnkEditorStorage : ISnkRuntimeStorage
        {
            public string[] PutObjects(List<string> keyList);
            public string[] DeleteObjects(List<string> keyList);
        }
    }
}