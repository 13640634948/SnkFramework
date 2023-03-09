using SnkFramework.NuGet.Logging;

namespace SnkFramework.Runtime.Engine
{
    public class SnkUnityLog : SnkConsoleLog
    {
        public SnkUnityLog(string name, SnkLogFactory factory) : base(name, factory)
        {
        }

        public override void Debug(object message)
        {
            UnityEngine.Debug.Log(message);
        }
        public override void Info(object message)
        {
            UnityEngine.Debug.Log(message);
        }
        public override void Warn(object message)
        {
            UnityEngine.Debug.LogWarning(message);
        }
        public override void Error(object message)
        {
            UnityEngine.Debug.LogError(message);
        }
        public override void Fatal(object message)
        {
            UnityEngine.Debug.LogError(message);
        }
    }
}