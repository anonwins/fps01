using UnityEditor;
using UnityEngine;

public static class BuildScript
{
    [MenuItem("Build/Build Windows (Standalone)")]
    public static void BuildWindows()
    {
        string[] scenes = FindEnabledEditorScenes();
        string buildPath = "Builds/Windows/";

        System.IO.Directory.CreateDirectory(buildPath);

        BuildPipeline.BuildPlayer(scenes, buildPath + "FPSPrototype.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
    }

    [MenuItem("Build/Build Windows (Development)")]
    public static void BuildWindowsDevelopment()
    {
        string[] scenes = FindEnabledEditorScenes();
        string buildPath = "Builds/Windows/";

        System.IO.Directory.CreateDirectory(buildPath);

        BuildPipeline.BuildPlayer(scenes, buildPath + "FPSPrototype.exe", BuildTarget.StandaloneWindows64, BuildOptions.Development);
    }

    private static string[] FindEnabledEditorScenes()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }
        return scenes;
    }

    [MenuItem("Build/Build WebGL")]
    public static void BuildWebGL()
    {
        string[] scenes = FindEnabledEditorScenes();
        string buildPath = "Builds/WebGL/";

        System.IO.Directory.CreateDirectory(buildPath);

        BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.WebGL, BuildOptions.None);
    }
}