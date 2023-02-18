using SnkFramework.NuGet.Preference;
using SnkFramework.Runtime.Basic.TypeEncoder;

namespace SnkFramework.Runtime.Preference
{
    public class SnkUnityPrefSerializer : SnkDefaultSerializer
    {
        public SnkUnityPrefSerializer()
        {
            this.AddTypeEncoder(new SnkRectTypeEncoder());
            this.AddTypeEncoder(new SnkColorTypeEncoder());
            this.AddTypeEncoder(new SnkVectorTypeEncoder());
        }
    }
}