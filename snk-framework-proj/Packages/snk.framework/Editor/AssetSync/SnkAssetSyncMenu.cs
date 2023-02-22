using SnkFramework.Network.ContentDelivery.Editor;
using SnkFramework.NuGet.Basic;
using UnityEditor;

namespace SnkFramework.Editor.AssetSync
{
    namespace Window
    {
        public class SnkAssetSyncMenu : EditorWindow
        {
            [MenuItem("Window/SnkFramework/AssetSync-SyncToRemote")]
            public static void SyncToRemote()
            {
                const string patchRepositoryPath = "Repository";
                var assetSyncer = new SnkAssetSyncer<SnkMD5Generator>(patchRepositoryPath);
                assetSyncer.LocalSyncToRemote<SnkCOSStorage>();
            }
        }
    }
}