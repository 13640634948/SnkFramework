using System;
using SnkFramework.NuGet.Features.Logging;
using UnityEngine;

namespace SnkFramework.Runtime
{
    namespace Engine
    {
        public class SnkUnityLoggerProvider : ISnkLoggerProvider
        {
            public void Output(eSnkLogType logType, Exception e, string formater, params object[] message)
            {
                if(e != null)
                    Debug.LogException(e);
                else
                {
                    switch (logType)
                    {
                        case eSnkLogType.error:
                            if(message == null || message.Length == 0)
                                Debug.LogError(formater);
                            else
                                Debug.LogErrorFormat(formater, message);   
                            break;
                        case eSnkLogType.warning:
                            Debug.LogWarningFormat(formater, message);
                            break;
                        default:

                            if (message == null || message.Length == 0)
                                Debug.Log(formater);
                            else
                                Debug.LogFormat(formater, message);
                            break;
                    }
                }
            }
        }
    }
}