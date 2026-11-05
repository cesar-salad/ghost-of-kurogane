using System.IO;
using GhostOfKurogane.CameraSystem;
using GhostOfKurogane.Combat;
using GhostOfKurogane.Core;
using GhostOfKurogane.Interaction;
using GhostOfKurogane.Player;
using GhostOfKurogane.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace GhostOfKurogane.Editor
{
    /// <summary>Creates real serialized assets through Unity APIs, always at unused paths.</summary>
    public static class PrototypeSceneBuilder
    {
        [MenuItem("Ghost of Kurogane/Create Prototype Playground")]
        public static void CreatePlayground()
        {
            if (!Application.isBatchMode && !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            ConfigureProject();
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            new GameObject("Game Manager").AddComponent<GameManager>();
            Primitive("Ground", PrimitiveType.Plane, Vector3.zero, new Vector3(3f, 1f, 3f));
            var player = new GameObject("Maddox (Placeholder)");
            player.transform.position = new Vector3(0f, 0.1f, 0f);
            var controller = player.AddComponent<CharacterController>();
            controller.height = 2f;
            controller.radius = 0.4f;
            controller.center = Vector3.up;
            controller.minMoveDistance = 0f;
            var visual = Primitive("Placeholder Body", PrimitiveType.Capsule, Vector3.zero, Vector3.one);
            visual.transform.SetParent(player.transform, false);
            visual.transform.localPosition = Vector3.up;
            Object.DestroyImmediate(visual.GetComponent<Collider>());
            var input = player.AddComponent<PlayerInputReader>();
            var movement = player.AddComponent<PlayerController>();
            player.AddComponent<Health>();
            var interactor = player.AddComponent<PlayerInteractor>();
            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            var camera = cameraObject.AddComponent<UnityEngine.Camera>();
            camera.nearClipPlane = 0.1f;
            cameraObject.AddComponent<AudioListener>();
            cameraObject.AddComponent<UniversalAdditionalCameraData>();
            cameraObject.AddComponent<ThirdPersonCamera>().Configure(player.transform, input);
            cameraObject.transform.SetPositionAndRotation(new Vector3(0f, 3f, -5f), Quaternion.Euler(15f, 0f, 0f));
            movement.Configure(input, camera.transform);
            new GameObject("Prototype HUD").AddComponent<PrototypeHud>().Configure(interactor);
            var sun = new GameObject("Directional Light").AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.intensity = 2f;
            sun.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
            sun.gameObject.AddComponent<UniversalAdditionalLightData>();
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.4f, 0.45f, 0.5f);
            Primitive("Low Obstacle", PrimitiveType.Cube, new Vector3(3f, 0.4f, 2f), new Vector3(2f, 0.8f, 2f));
            Primitive("Wall", PrimitiveType.Cube, new Vector3(-4f, 1.5f, 3f), new Vector3(1f, 3f, 5f));
            Primitive("Platform", PrimitiveType.Cube, new Vector3(4f, 0.75f, 6f), new Vector3(3f, 1.5f, 3f));
            var terminal = Primitive("Test Terminal", PrimitiveType.Cube, new Vector3(0f, 1f, 2f), new Vector3(1f, 2f, 0.6f));
            terminal.AddComponent<ExampleInteractable>();
            Directory.CreateDirectory("Assets/_Game/Scenes/Prototypes");
            AssetDatabase.Refresh();
            string path = AssetDatabase.GenerateUniqueAssetPath("Assets/_Game/Scenes/Prototypes/Prototype_Playground.unity");
            if (!EditorSceneManager.SaveScene(scene, path)) throw new IOException("Could not save prototype scene.");
            AssetDatabase.SaveAssets();
            Debug.Log($"Created {path}. Existing scenes and Build Profiles were preserved.");
        }

        /// <summary>Initial project defaults; existing render-pipeline configuration is preserved.</summary>
        public static void ConfigureProject()
        {
            EditorSettings.serializationMode = SerializationMode.ForceText;
            VersionControlSettings.mode = "Visible Meta Files";
            if (GraphicsSettings.defaultRenderPipeline != null) return;
            const string settingsFolder = "Assets/_Game/Settings";
            Directory.CreateDirectory(settingsFolder);
            AssetDatabase.Refresh();
            var renderer = ScriptableObject.CreateInstance<UniversalRendererData>();
            AssetDatabase.CreateAsset(renderer, AssetDatabase.GenerateUniqueAssetPath(settingsFolder + "/Prototype_Renderer.asset"));
            var pipeline = UniversalRenderPipelineAsset.Create(renderer);
            AssetDatabase.CreateAsset(pipeline, AssetDatabase.GenerateUniqueAssetPath(settingsFolder + "/Prototype_URP.asset"));
            GraphicsSettings.defaultRenderPipeline = pipeline;
            QualitySettings.renderPipeline = pipeline;
        }

        private static GameObject Primitive(string name, PrimitiveType type, Vector3 position, Vector3 scale)
        {
            var result = GameObject.CreatePrimitive(type);
            result.name = name;
            result.transform.position = position;
            result.transform.localScale = scale;
            return result;
        }
    }
}
