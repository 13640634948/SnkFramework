using System;
using System.Collections.Generic;
using SnkFramework.Runtime.Editor.PlayerBuilder;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class BuildWindow : EditorWindow
{
    [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/BuildWindow")]
    public static void ShowExample()
    {
        BuildWindow wnd = GetWindow<BuildWindow>();
        wnd.titleContent = new GUIContent("BuildWindow");
    }

    public string[] SnkPlatform =
    {
        "Droid",
        "iOS",
        "OSX",
        "Windows"
    };

    public string[] SnkChannel =
    {
        "BackFire-BFSDK",
        "Hero(USDK)",
    };

    public string[] SnkTargetType =
    {
        "Alpha", //内部测试版：bug会比较多，功能不全，仅提供给内部测试人员
        "Bata", //公开测试版：核心用户的小范围测试使用，相对于alpha要更稳定
        "Rc", //发行候选版：仅修复bug
        "Final", //正式发布版：最后生成的版本
    };

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        //VisualElement label = new Label("Hello World! From C#");
        //root.Add(label);

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        var platform = root.Q<DropdownField>("Platform");
        platform.choices = new List<string>(SnkPlatform);

        var channel = root.Q<DropdownField>("Channel");
        channel.choices = new List<string>(SnkChannel);

        var targetType = root.Q<DropdownField>("TargetType");
        targetType.choices = new List<string>(SnkTargetType);

        root.Q<Button>().clicked += () =>
        {
            var buildTarget = BuildTarget.NoTarget;
            if (platform.value == SnkPlatform[0])
            {
                buildTarget = BuildTarget.Android;
            }
            else if (platform.value == SnkPlatform[1])
            {
                buildTarget = BuildTarget.iOS;
            }
            else if (platform.value == SnkPlatform[2])
            {
                buildTarget = BuildTarget.StandaloneOSX;
            }
            else if (platform.value == SnkPlatform[3])
            {
                buildTarget = BuildTarget.StandaloneWindows64;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

            if (buildTarget == BuildTarget.NoTarget)
            {
                Debug.LogError("构建平台错误:" + buildTarget);
                return;
            }

            PlayerBuilder.Build(buildTarget);
        };
    }
}