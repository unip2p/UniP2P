using UnityEditor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BuildClass
{
    private static EditorBuildSettingsScene[] GetEditorBuildSettingsScenes()
    {
        // ビルド対象シーンリスト
        List<EditorBuildSettingsScene> allScene = new List<EditorBuildSettingsScene>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                allScene.Add(scene);
            }
        }
        return allScene.ToArray();
    }
    
    private static string DoBashCommand(string cmd)
    {
        var p = new Process();
        p.StartInfo.FileName = "/bin/bash";
        p.StartInfo.Arguments = "-c \" " + cmd + " \"";
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.Start();

        var output = p.StandardOutput.ReadToEnd();
        p.WaitForExit();
        p.Close();

        return output;
    }

    private static string GetProjectRoot()
    {
        var path = Application.dataPath;
        var s = path.Length - 7;
        var root = path.Remove(s);
        return root;
    }

    [MenuItem("UniP2P/Build and Double Run")]
    public static void Build()
    {
        // 実行
#if UNITY_EDITOR_WIN
        var filename = GetProjectRoot() + "/Build/" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + "/" +
               Application.productName + ".exe";
        BuildPipeline.BuildPlayer(GetEditorBuildSettingsScenes(), filename, BuildTarget.StandaloneWindows64, BuildOptions.None);
        Process.Start(filename);
        Process.Start(filename);
#elif UNITY_EDITOR_MAC
        var filename = GetProjectRoot() + "/Build/" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + "/" +
                       Application.productName + ".app";
        BuildPipeline.BuildPlayer(GetEditorBuildSettingsScenes(), filename, BuildTarget.StandaloneOSX, BuildOptions.None);
        DoBashCommand("open -n " + filename);
        DoBashCommand("open -n " + filename);   
#endif
    }

    [MenuItem("UniP2P/Build and Run")]
    public static void SingleBuild()
    {
#if UNITY_EDITOR_WIN
        var filename = GetProjectRoot() + "/Build/" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + "/" +
               Application.productName + ".exe";
        BuildPipeline.BuildPlayer(GetEditorBuildSettingsScenes(), filename, BuildTarget.StandaloneWindows64, BuildOptions.None);
        Process.Start(filename);
#elif UNITY_EDITOR_MAC
        var filename = GetProjectRoot() + "/Build/" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + "/" +
                       Application.productName + ".app";
        BuildPipeline.BuildPlayer(GetEditorBuildSettingsScenes(), filename, BuildTarget.StandaloneOSX, BuildOptions.None);
        DoBashCommand("open -n " + filename);
#endif
    }
}
