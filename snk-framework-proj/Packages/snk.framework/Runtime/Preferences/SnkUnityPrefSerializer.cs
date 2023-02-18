namespace SnkFramework.NuGet.Preference
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