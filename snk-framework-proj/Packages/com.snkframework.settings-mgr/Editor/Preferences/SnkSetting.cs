using UnityEditor;
using UnityEditor.SettingsManagement;

namespace SnkFramework.SettingsManager.Editor
{
    // Usually you will only have a single Settings instance, so it is convenient to define a UserSetting<T> implementation
    // that points to your instance. In this way you avoid having to pass the Settings parameter in setting field definitions.
    class SnkSetting<T> : UserSetting<T>
    {
        public SnkSetting(string key, T value, SettingsScope scope = SettingsScope.Project)
            : base(SnkSettingsManager.instance, key, value, scope)
        {}

        SnkSetting(Settings settings, string key, T value, SettingsScope scope = SettingsScope.Project)
            : base(settings, key, value, scope) { }
    }
}
