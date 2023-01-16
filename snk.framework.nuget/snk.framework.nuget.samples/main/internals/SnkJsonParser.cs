using Newtonsoft.Json;
using snk.framework.nuget.basic;

namespace SnkFramework.Nuget.Sameples
{
    namespace Internals
    {
        internal class SnkJsonParser : ISnkJsonParser
        {
            public T FromJson<T>(string json) where T : class => JsonConvert.DeserializeObject<T>(json);

            public string ToJson(object target) => JsonConvert.SerializeObject(target);
        }
    }
}
