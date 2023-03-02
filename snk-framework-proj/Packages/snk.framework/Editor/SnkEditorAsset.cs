using UnityEditor;
using UnityEngine;

namespace SnkFramework.Editor
{
    public abstract class SnkEditorAsset<T> : ScriptableObject
        where T : UnityEngine.Object
    {
        private static T _settings;
        public static T GetInstance(string tag = "")
        {
            if (_settings == null)
            {
                var assetPath = $"Assets/Editor/SnkFramework/{typeof(T).Name}{tag}.asset";
                _settings = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (_settings == null)
                {
                    Debug.LogError("路径下没有资源.path:" + assetPath);
                }
            }
            return _settings;
        }
    }
}