using System;
using System.Collections;
using SnkFramework.Mvvm.Core;
using UnityEngine;

namespace SampleDevelop.Mvvm
{
    public class DemoCoroutineExecutor : MonoBehaviour, ISnkMvvmCoroutineExecutor
    {
        public void RunOnCoroutineNoReturn(IEnumerator routine) => StartCoroutine(routine);
    }
}