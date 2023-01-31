using UnityEditor;
using UnityEditor.SettingsManagement;

namespace SnkFramework.SettingsManager.Editor
{
    /// <summary>
    /// This class will act as a manager for the <see cref="Settings"/> singleton.
    /// </summary>
    static class SnkSettingsManager
    {
        // Replace this with your own package name. Project settings will be stored in a JSON file in a directory matching
        // this name.
        //internal const string k_PackageName = "com.unity.settings-manager-examples";
        internal const string k_PackageName = "com.snkframework.settings";

        static Settings s_Instance;

        internal static Settings instance
        {
            get
            {
                if (s_Instance == null)
                    s_Instance = new Settings(k_PackageName);

                return s_Instance;
            }
        }

        // The rest of this file is just forwarding the various setting methods to the instance.

        public static void Save()
        {
            instance.Save();
        }

        public static T GetProj<T>(string key, T fallback = default(T))
        {
            return instance.Get<T>(key, SettingsScope.Project, fallback);
        }

        public static void SetProj<T>(string key, T value)
        {
            instance.Set<T>(key, value, SettingsScope.Project);
        }

        public static bool ContainsKeyProj<T>(string key)
        {
            return instance.ContainsKey<T>(key, SettingsScope.Project);
        }

        public static T GetUser<T>(string key, T fallback = default(T))
        {
            return instance.Get<T>(key, SettingsScope.User, fallback);
        }

        public static void SetUser<T>(string key, T value)
        {
            instance.Set<T>(key, value, SettingsScope.User);
        }

        public static bool ContainsKeyUser<T>(string key)
        {
            return instance.ContainsKey<T>(key, SettingsScope.User);
        }
    }
}