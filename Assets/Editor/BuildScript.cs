using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

// Command-line build entry points. Run via:
//   Unity.exe -quit -batchmode -projectPath <proj> -executeMethod BuildScript.BuildWindows
public class BuildScript
{
    static string[] Scenes()
    {
        return EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
    }

    public static void BuildWindows()
    {
        var opts = new BuildPlayerOptions
        {
            scenes = Scenes(),
            locationPathName = "Build/Windows/The EYE.exe",
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };
        var report = BuildPipeline.BuildPlayer(opts);
        Debug.Log("Windows build result: " + report.summary.result + " size=" + report.summary.totalSize);
        if (report.summary.result != BuildResult.Succeeded)
            EditorApplication.Exit(1);
    }

    public static void BuildWebGL()
    {
        // Disabled compression so it works on itch without server config.
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
        var opts = new BuildPlayerOptions
        {
            scenes = Scenes(),
            locationPathName = "Build/Web",
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };
        var report = BuildPipeline.BuildPlayer(opts);
        Debug.Log("WebGL build result: " + report.summary.result + " size=" + report.summary.totalSize);
        if (report.summary.result != BuildResult.Succeeded)
            EditorApplication.Exit(1);
    }
}
