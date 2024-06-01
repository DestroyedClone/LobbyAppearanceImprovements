using R2API;
using RoR2;
using RoR2.UI;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static LobbyAppearanceImprovements.ConfigSetup;

namespace LobbyAppearanceImprovements.Scenes
{
    public abstract class LAIScene
    {
        public LAIScene()
        {
        }

        public static Action<LAIScene> onSceneLoaded;
        public static Action<LAIScene> onSceneUnloaded;
        public abstract string SceneNameToken { get; }
        public abstract GameObject BackgroundPrefab { get; }
        public abstract Vector3 Position { get; }
        public abstract Quaternion Rotation { get; }
        public abstract Vector3 Scale { get; }
        public virtual string PreferredLayout { get; }
        public virtual string[] RequiredModGUIDs { get; }
        public bool HasSetup = false;
        public virtual Material SkyboxOverride { get; } = defaultSkyboxMaterial;

        public static Material defaultSkyboxMaterial = LoadAsset<Material>("RoR2/Base/Common/Skyboxes/matSkybox1.mat");
        public virtual string Credit { get; }
        public virtual string MusicTrackName { get; } = null;
        public virtual MusicTrackDef MusicTrackDef { get; set; } = defaultMusicTrackDef;
        public static MusicTrackDef defaultMusicTrackDef = LoadAsset<MusicTrackDef>("RoR2/Base/Common/muLogbook");

        public virtual void Init()
        {
            var nameOfThis = GetType().Name;
            LAILogging.LogMessage($"{nameOfThis}.Init :: Setting up scene.", LoggingStyle.UserShouldSee);
            if (HasSetup)
            {
                LAILogging.LogMessage($"{nameOfThis}.Init :: Ran Init(), but has already set up!", LoggingStyle.UserShouldSee);
                return;
            }
            HasSetup = true;
            LAISceneManager.onVoteStarted += OnVoteStarted;
        }

        public virtual void OnVoteStarted(LAIScene scene)
        {
        }

        public bool CanLoadScene()
        {
            if (RequiredModGUIDs != null && RequiredModGUIDs.Length > 0)
            {
                foreach (var GUID in RequiredModGUIDs) //Todo: Add optional assembly: "a.b.c||a.b.d"
                {
                    if (!BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(GUID))
                    {
                        LAILogging.LogMessage($"Refused to load scene \"{GetType().Name}\" because GUID \"{GUID}\" was not loaded!", LoggingStyle.UserShouldSee);
                        return false;
                    }
                }
            }
            return true;
        }

        public bool IsSceneOfType<T>()
        {
            return this is T;
        }

        public GameObject CreateScene(bool selectScene)
        {
            GameObject sceneInstance = null;
            if (BackgroundPrefab)
            {
                sceneInstance = UnityEngine.Object.Instantiate<GameObject>(BackgroundPrefab);
                sceneInstance.transform.SetPositionAndRotation(Position, Rotation);
                sceneInstance.transform.localScale = Scale;
            }
            if (selectScene)
            {
                LAISceneManager.sceneInstance = sceneInstance;
                HookMethods.Hook_Overlay_ShowPostProcessing(PostProcessing.Value);
                onSceneLoaded?.Invoke(this);
            }
            return sceneInstance;
        }

        public void OnDestroy()
        {
            onSceneUnloaded?.Invoke(this);
        }
        public static T LoadAsset<T>(string path)
        {
            return Addressables.LoadAssetAsync<T>(path).WaitForCompletion();
        }

        public static GameObject LoadAsset(string path)
        {
            return LoadAsset<GameObject>(path);
        }

        public static GameObject PrefabCloneFromAddressable(string path, string name)
        {
            return PrefabAPI.InstantiateClone(LoadAsset(path), name, false);
        }

        public static GameObject CloneFromAddressable(string path, Transform parentTransform = null)
        {
            var obj = UnityEngine.Object.Instantiate(LoadAsset(path));
            if (parentTransform) obj.transform.parent = parentTransform;
            return obj;
        }

        public string SceneTitleToken
        {
            get
            {
                return SceneNameToken + "_TITLE";
            }
        }
        public string SceneSubtitleToken
        {
            get
            {
                return SceneNameToken + "_SUBTITLE";
            }
        }

        public class PrefabSpawner : MonoBehaviour
        {
            public string AssetPath;
            public Vector3 localPosition;
            //public Vector3 localPosition;
            public Quaternion rotation;

            public GameObject spawnedObject;

            public bool hasSpawned = false;

            public void Awake()
            {
                //Position becomes localposition after use
                spawnedObject = LAIScene.CloneFromAddressable(AssetPath, this.transform);
                if (rotation != null) spawnedObject.transform.rotation = rotation;
                if (localPosition != null) spawnedObject.transform.position = localPosition;
                //if (localPosition != null) spawn.transform.localPosition = position;
                enabled = false;
            }

            public void Editor_RespawnObject()
            {
                Destroy(spawnedObject);
                Awake();
            }
        }
    }
}