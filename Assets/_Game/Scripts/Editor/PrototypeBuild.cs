using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace GhostOfKurogane.Editor
{
    /// <summary>Builds the saved playground without changing the project's Build Profile scene list.</summary>
    public static class PrototypeBuild
    {
        private const string PrototypeScene = "Assets/_Game/Scenes/Prototypes/Prototype_Playground.unity";

        /// <summary>Creates a uniquely named Windows development build and a short evidence report.</summary>
        [MenuItem("Ghost of Kurogane/Build Windows Baseline")]
        public static void BuildWindowsBaseline()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || BuildPipeline.isBuildingPlayer)
                throw new BuildFailedException("Exit Play mode and wait for any current build before building the baseline.");
            if (!File.Exists(PrototypeScene))
                throw new BuildFailedException($"Open or generate {PrototypeScene} before building the baseline.");
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64))
                throw new BuildFailedException("Install Windows Build Support for this Editor in Unity Hub before building the baseline.");
            if (!Application.isBatchMode && !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

            string projectRoot = Path.GetDirectoryName(Application.dataPath);
            string buildName = $"Baseline-{DateTime.UtcNow:yyyyMMdd-HHmmss}-{Guid.NewGuid():N}";
            string outputDirectory = Path.Combine(projectRoot, "Builds", "Windows", buildName);
            Directory.CreateDirectory(outputDirectory);
            string executablePath = Path.Combine(outputDirectory, "GhostOfKurogane.exe");
            var options = new BuildPlayerOptions
            {
                scenes = new[] { PrototypeScene },
                locationPathName = executablePath,
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.Development | BuildOptions.StrictMode
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            var summary = report.summary;
            string evidence = $"Ghost of Kurogane - M0 Windows baseline\n" +
                $"UTC: {DateTime.UtcNow:O}\nUnity: {Application.unityVersion}\n" +
                $"Scene: {PrototypeScene}\nTarget: {summary.platform}\n" +
                $"Result: {summary.result}\nErrors: {summary.totalErrors}\nWarnings: {summary.totalWarnings}\n" +
                $"Duration: {summary.totalTime}\nBytes: {summary.totalSize}\n" +
                $"Executable: {executablePath}\n" +
                "Development build; visual and input acceptance must be checked separately.\n";
            File.WriteAllText(Path.Combine(outputDirectory, "build-evidence.txt"), evidence);
            Debug.Log(evidence);
            if (summary.result != BuildResult.Succeeded)
                throw new BuildFailedException($"Baseline build failed. Inspect {outputDirectory}/build-evidence.txt and the Editor log.");
        }
    }
}