using UnityEditor;
using UnityEngine;

namespace SnkFramework.Network.FileStorage
{
    namespace Editor
    {
        public abstract class SnkStorageSettings
        {
            public bool mIsEnable;

            public string mEndPoint;
            public string mAccessKeyId;
            public string mAccessKeySecret;
            public string mBucketName;
            public bool mIsQuietDelete;

            public static TStorageSettings Load<TStorageSettings>() where TStorageSettings : SnkStorageSettings, new()
            {
                string key = typeof(TStorageSettings).Name;
                string json = EditorPrefs.GetString(key, string.Empty);
                if (string.IsNullOrEmpty(json))
                {
                    var settings = new TStorageSettings();
                    json = JsonUtility.ToJson(settings);
                    EditorPrefs.SetString(key, json);
                }

                return JsonUtility.FromJson<TStorageSettings>(json);
            }
        }
    }
}