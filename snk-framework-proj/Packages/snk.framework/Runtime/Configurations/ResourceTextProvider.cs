using SnkFramework.NuGet;
using SnkFramework.NuGet.Basic;
using SnkFramework.NuGet.Logging;
using UnityEngine;

namespace SnkFramework.Runtime.Configurations
{
    public class ResourceTextProvider : ISnkTextProvider
    {
        private static ISnkLog log = SnkLogHost.GetLogger<ResourceTextProvider>();
        private readonly string _assetPath;
        public ResourceTextProvider(string assetPath)
        {
            _assetPath = assetPath;
        }

        public string Load()
        {
            var textAsset = Resources.Load<TextAsset>(_assetPath);
            if(textAsset == null)
                log?.Warn($"found out TextAsset. assetPath:{_assetPath}");
            return textAsset?.text;
        }
    }
}