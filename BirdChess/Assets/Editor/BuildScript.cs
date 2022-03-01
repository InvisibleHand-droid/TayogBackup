using UnityEngine;
using UnityEditor;

// Can be modified and extended to support other platforms
public class BuildScript
{
    /*// Edit these values according to your preferences
    static string androidPath = "./Builds/Android/Immortal Ranker " + Application.version + ".apk"; 
    static string windowsPath = "./Builds/Windows/Immortal Ranker " + Application.version + ".exe";

    static EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

    [MenuItem("File/Build All")]
    static void BuildAll()
    {
        var scenes = EditorBuildSettings.scenes;
        BuildPipeline.BuildPlayer(scenes, "./Builds/Windows/Shift (Alpha).exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
        BuildPipeline.BuildPlayer(scenes, "./Builds/Linux/Shift (Alpha).x86_64", BuildTarget.StandaloneLinux64, BuildOptions.None);
        BuildPipeline.BuildPlayer(scenes, "./Builds/MacOSX.app/Shift (Alpha).x64", BuildTarget.StandaloneOSX, BuildOptions.None);
        BuildPipeline.BuildPlayer(scenes, "./Builds/Web GL", BuildTarget.WebGL, BuildOptions.None);
    }

    [MenuItem("File/Build Windows64Bit")]
    static void BuildWindows()
    {
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, windowsPath, BuildTarget.StandaloneWindows64, BuildOptions.None);
    }

    [MenuItem("File/Build Linux")]
    static void BuildLinux()
    {
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, "./Builds/Linux/Linux.x86_64", BuildTarget.StandaloneLinux64, BuildOptions.None);
    }

    [MenuItem("File/Build OS X")]
    static void BuildOSX()
    {
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, "./Builds/MacOSX.app/Mac.x64", BuildTarget.StandaloneOSX, BuildOptions.None);
    }

    [MenuItem("File/Build WebGL")]
    static void BuildWebGL()
    {
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, "./Builds/Web GL", BuildTarget.WebGL, BuildOptions.None);
    }

    [MenuItem("File/Build Android")]
    static void BuildAndroid()
    {
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, androidPath, BuildTarget.Android, BuildOptions.None);
    }

    [MenuItem("File/Build Windows64Bit And Android")]
    static void BuildWindows64BitAndAndroid()
    {
        BuildPipeline.BuildPlayer(scenes, windowsPath, BuildTarget.StandaloneWindows64, BuildOptions.None);
        BuildPipeline.BuildPlayer(scenes, androidPath, BuildTarget.Android, BuildOptions.None);
    }

    static void PerformAssetBundleBuild()
    {
        BuildPipeline.BuildAssetBundles("../AssetBundles/", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneLinux64);
        BuildPipeline.BuildAssetBundles("../AssetBundles/", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
        BuildPipeline.BuildAssetBundles("../AssetBundles/", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneOSX);
        BuildPipeline.BuildAssetBundles("../AssetBundles/", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.WebGL);
        BuildPipeline.BuildAssetBundles("../AssetBundles/", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
    }*/
}