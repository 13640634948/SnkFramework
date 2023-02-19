using SnkFramework.NuGet.Basic;
using SnkFramework.NuGet.Features.Logging;
using UnityEngine;

namespace SnkFramework.Runtime.Configurations
{
    public class ResourceTextProvider : ISnkTextProvider
    {
        private static ISnkLogger log = SnkLogHost.GetLog<ResourceTextProvider>();
        private readonly string _assetPath;
        public ResourceTextProvider(string assetPath)
        {
            _assetPath = assetPath;
        }

        public string Load()
        {
            var textAsset = Resources.Load<TextAsset>(_assetPath);
            if(textAsset == null)
                log?.Warning("found out TextAsset. assetPath:" + _assetPath);
            return textAsset?.text;
        }
    }
}