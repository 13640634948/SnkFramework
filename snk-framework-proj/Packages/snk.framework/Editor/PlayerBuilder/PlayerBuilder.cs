using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace SnkFramework.Runtime.Editor.PlayerBuilder
{
    public class PlayerBuilder
    {
        public static void Build(string exportFolder, BuildTarget buildTarget)
        {
            exportFolder = Path.Combine(exportFolder, buildTarget.ToString(), "proj-v");
            if (Directory.Exists(exportFolder) == false)
                Directory.CreateDirectory(exportFolder);
            
            
                
            //获取场景编译场景列表
            var scenes = EditorBuildSettings.scenes;
            var result = BuildPipeline.BuildPlayer(scenes, exportFolder, buildTarget, BuildOptions.None);
        }
    }
}