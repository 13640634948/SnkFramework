using UnityEngine;

namespace SnkFramework.Editor
{
    [CreateAssetMenu(menuName = "SnkFramework/Create BuildSettings", fileName = "SnkBuildSettings")]
    public class SnkBuildSettings : SnkEditorAsset<SnkBuildSettings>
    {
        [Header("包体导出路径")]
        public string exportFolder;
    }
}