using System;
using SnkFramework.NuGet.Features.Logging;
using UnityEngine;

namespace SnkFramework.Runtime
{
    namespace Engine
    {
        public class UnityLoggerProvider : ISnkLoggerProvider
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
                            Debug.LogErrorFormat(formater, message);
                            break;
                        case eSnkLogType.warning:
                            Debug.LogWarningFormat(formater, message);
                            break;
                        default:
                            Debug.LogFormat(formater, message);
                            break;
                    }
                }
            }
        }
    }
}