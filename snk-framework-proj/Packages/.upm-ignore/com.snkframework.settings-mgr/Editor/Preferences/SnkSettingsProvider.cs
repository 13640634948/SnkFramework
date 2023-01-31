using UnityEditor;
using UnityEditor.SettingsManagement;

namespace SnkFramework.SettingsManager.Editor
{
	/// <summary>
	/// To create an entry in the Preferences window, define a new SettingsProvider inheriting <see cref="UserSettingsProvider"/>.
	/// You can also choose to implement your own SettingsProvider and ignore this implementation. The benefit of using
	/// <see cref="UserSettingsProvider"/> is that all <see cref="UserSetting{T}"/> fields in the assembly are automatically
	/// populated within the preferences, with support for search and resetting default values.
	/// </summary>
	static class SnkSettingsProvider
	{
		const string k_PreferencesPath = "Preferences/SnkFramework";
		[SettingsProvider]
		static SettingsProvider CreateSettingsProvider()
		{
			var provider = new UserSettingsProvider(k_PreferencesPath,
				SnkSettingsManager.instance,
				new [] { typeof(SnkSettingsProvider).Assembly });

			return provider;
		}
	}
}
