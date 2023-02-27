using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace SnkFramework.Runtime.Editor.PlayerBuilder
{
    public class PlayerBuilder
    {
        public static void Build(BuildTarget buildTarget)
        {
            var exportFolder = Path.GetFullPath(".") + "/PlayerOutput/" + buildTarget;
            if (!Directory.Exists(exportFolder))
            {
                Directory.CreateDirectory(exportFolder);
            }
 
            //提供用户选择路径
            var selectPath = EditorUtility.SaveFolderPanel("请选择保存路径", exportFolder,"");
            //用户点击取消处理
            if (string.IsNullOrEmpty(selectPath))
            {
                Debug.Log("UPublish:用户取消导出GVRSDK  AS项目工程");
                return;
            }
 
            //获取场景编译场景列表
            var scenes = EditorBuildSettings.scenes;
            Debug.Log("UPublish:开始发布导出GVRSDK AS项目工程");
 
            BuildReport result = BuildPipeline.BuildPlayer(scenes, selectPath, buildTarget, BuildOptions.None);

        }
    }
}