using SnkFramework.Network.ContentDelivery.Editor;
using SnkFramework.NuGet.Basic;
using UnityEditor;

namespace BFFramework.Editor
{
    public class SnkAssetSyncMenu
    {
        [MenuItem("Window/SnkFramework/AssetSync-SyncToRemote")]
        public static void MenuSyncToRemote()
        {
            const string patchRepositoryPath = "Repository";
            SyncToRemote(patchRepositoryPath);
        }

        public static void SyncToRemote(string patchRepositoryPath)
        {
            var assetSyncer = new SnkAssetSyncer<SnkMD5Generator>(patchRepositoryPath);
            assetSyncer.LocalSyncToRemote<SnkCOSStorage>();
        }
    }
}