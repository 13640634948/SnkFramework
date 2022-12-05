using SnkFramework.CloudRepository.Editor.Settings;
using UnityEditor;
using UnityEngine;

namespace SnkFramework.CloudRepository.Editor
{
    namespace Window
    {
        public class CloudRepositoryWindow : EditorWindow
        {
            private SnkOSSStorageSettings _mOssSetting = new();
            private SnkCOSStorageSettings _mCosSetting = new();
            private SnkOBSStorageSettings _mObsSetting = new();

            [MenuItem("Window/SnkFramework/Cloud Repository")]
            public static void ShowWindow()
            {
                var window =
                    GetWindow<CloudRepositoryWindow>(L10n.Tr("Cloud Repository"), typeof(CloudRepositoryWindow));
                window.minSize = new Vector2(445, 385);
                window.Show();
            }

            private void SaveStorageSettings(SnkStorageSettings storageSettings)
            {
                string key = storageSettings.GetType().Name;
                string json = JsonUtility.ToJson(storageSettings);
                EditorPrefs.SetString(key, json);
            }

            private void InitCloudRepositoryWindow()
            {
                _mOssSetting = SnkStorageSettings.Load<SnkOSSStorageSettings>();
                _mCosSetting = SnkStorageSettings.Load<SnkCOSStorageSettings>();
                _mObsSetting = SnkStorageSettings.Load<SnkOBSStorageSettings>();
            }

            private void SaveCloudRepositoryWindow()
            {
                this.SaveStorageSettings(_mOssSetting);
                this.SaveStorageSettings(_mCosSetting);
                this.SaveStorageSettings(_mObsSetting);
            }

            private void OnEnable()
            {
                InitCloudRepositoryWindow();
            }

            private void OnGUISnkStorageSettings(SnkStorageSettings settings)
            {
                settings.mBucketName = EditorGUILayout.TextField("BucketName", settings.mBucketName);
                settings.mEndPoint = EditorGUILayout.TextField("EndPoint", settings.mEndPoint);
                settings.mAccessKeyId = EditorGUILayout.TextField("AccessKeyId", settings.mAccessKeyId);
                settings.mAccessKeySecret = EditorGUILayout.TextField("AccessKeySecret", settings.mAccessKeySecret);
                settings.mIsQuietDelete = EditorGUILayout.Toggle("QuietDelete", settings.mIsQuietDelete);
            }

            public void OnGUI()
            {
                if (GUILayout.Button("保存"))
                {
                    SaveCloudRepositoryWindow();
                }

                this._mCosSetting.mIsEnable = EditorGUILayout.ToggleLeft("腾讯云-对象存储-设置", this._mCosSetting.mIsEnable);
                using (var ds = new EditorGUI.DisabledScope(this._mCosSetting.mIsEnable == false))
                {
                    OnGUISnkStorageSettings(this._mCosSetting);
                    this._mCosSetting.mDurationSecond = EditorGUILayout.LongField("mDurationSecond", _mCosSetting.mDurationSecond);
                }

                GUILayout.Space(18);

                this._mObsSetting.mIsEnable = EditorGUILayout.ToggleLeft("华为云-对象存储-设置", this._mObsSetting.mIsEnable);
                using (var ds = new EditorGUI.DisabledScope(this._mObsSetting.mIsEnable == false))
                {
                    OnGUISnkStorageSettings(_mObsSetting);
                }

                GUILayout.Space(18);

                this._mOssSetting.mIsEnable = EditorGUILayout.ToggleLeft("阿里云-对象存储-设置", this._mOssSetting.mIsEnable);
                using (var ds = new EditorGUI.DisabledScope(this._mOssSetting.mIsEnable == false))
                {
                    OnGUISnkStorageSettings(_mOssSetting);
                    this._mOssSetting.mBuffSize = EditorGUILayout.IntField("mBuffSize", _mOssSetting.mBuffSize);
                }
            }
        }
    }
}