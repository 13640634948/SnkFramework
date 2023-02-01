using Newtonsoft.Json;
using SnkFramework.NuGet.Basic;

namespace SnkFramework.Runtime.Basic
{
    public class SnkJsonParser : ISnkJsonParser
    {
        public T FromJson<T>(string json) where T : class => JsonConvert.DeserializeObject<T>(json);

        public string ToJson(object target) => JsonConvert.SerializeObject(target);
    }
}