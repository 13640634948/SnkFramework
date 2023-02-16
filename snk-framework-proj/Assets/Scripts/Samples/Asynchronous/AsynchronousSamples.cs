using BFFramework.Runtime.Managers;
using UnityEngine;


public class AsynchronousSamples : MonoBehaviour
{
    private BFAsyncManager _asyncMgr;
    void Start()
    {
        _asyncMgr = new BFAsyncManager();
    }
}
