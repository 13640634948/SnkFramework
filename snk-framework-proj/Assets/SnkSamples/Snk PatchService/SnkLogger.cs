using SnkFramework.NuGet.Basic;
using UnityEngine;

internal class SnkLogger : ISnkLogger
{
    public void Print(string message)
    {
        Debug.Log(message);
    }
}