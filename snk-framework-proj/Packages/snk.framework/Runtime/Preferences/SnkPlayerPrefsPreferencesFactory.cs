using SnkFramework.NuGet.Preference;

namespace SnkFramework.Runtime.Preference
{
    /// <summary>
    /// 
    /// </summary>
    public class SnkPlayerPrefsPreferencesFactory : SnkAbstractPreferenceFactory
    {
        /// <summary>
        /// 
        /// </summary>
        public SnkPlayerPrefsPreferencesFactory() : this(null, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializer"></param>
        public SnkPlayerPrefsPreferencesFactory(ISnkPreferenceSerializer serializer) : this(serializer, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="encryptor"></param>
        public SnkPlayerPrefsPreferencesFactory(ISnkPreferenceSerializer serializer, ISnkPreferenceEncryptor encryptor) : base(serializer, encryptor)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override SnkPreference Create(string name)
        {
            return new SnkPlayerPrefsPreferences(name, this.Serializer, this.Encryptor);
        }
    }
}