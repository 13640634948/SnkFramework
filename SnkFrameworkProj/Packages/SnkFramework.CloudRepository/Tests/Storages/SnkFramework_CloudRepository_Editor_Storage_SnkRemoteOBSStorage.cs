using System.Collections;
using NUnit.Framework;
using SnkFramework.CloudRepository.Editor.Storage;
using SnkFramework.CloudRepository.Runtime.Base;
using UnityEngine.TestTools;

public class SnkFramework_CloudRepository_Editor_Storage_SnkRemoteOBSStorage
{
    private static SnkRemoteStorageSettings _settings = new ()
    {
        endPoint = "",
        accessKeyId = "",
        accessKeySecret = "",
        bucketName = "",
    };
    
    [Test]
    public void SnkFramework_CloudRepository_Editor_Storage_SnkRemoteOBSStorageSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator SnkFramework_CloudRepository_Editor_Storage_SnkRemoteOBSStorageWithEnumeratorPasses()
    {
        var ss = new SnkRemoteOBSStorage(_settings);
        //ss.TakeObjects();
        yield return null;
    }
}