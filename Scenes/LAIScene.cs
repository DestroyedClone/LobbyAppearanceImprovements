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

        public abstract string SceneName { get; }
        public abstract string SceneNameToken { get; }
        public abstract GameObject BackgroundPrefab { get; }
        public abstract Vector3 Position { get; }
        public abstract Quaternion Rotation { get; }
        public abstract Vector3 Scale { get; }
        public virtual string PreferredLayout { get; }
        public virtual string[] RequiredModGUIDs { get; }
        public bool HasSetup = false;

        public virtual void Init()
        {
            LAILogging.LogMessage($"{SceneName}.Init :: Setting up scene.", LoggingStyle.UserShouldSee);
            if (HasSetup)
            {
                LAILogging.LogMessage($"{SceneName}.Init :: Ran Init(), but has already set up!", LoggingStyle.UserShouldSee);
                return;
            }
            HasSetup = true;
        }

        public bool CanLoadScene()
        {
            if (RequiredModGUIDs != null && RequiredModGUIDs.Length > 0)
            {
                foreach (var GUID in RequiredModGUIDs) //Todo: Add optional assembly: "a.b.c||a.b.d"
                {
                    if (!BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(GUID))
                    {
                        LAILogging.LogMessage($"Refused to load scene \"{SceneName}\" because GUID \"{GUID}\" was not loaded!", LoggingStyle.UserShouldSee);
                        return false;
                    }
                }
            }
            return true;
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

        public static GameObject LoadAsset(string path)
        {
            return Addressables.LoadAssetAsync<GameObject>(path).WaitForCompletion();
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
    }
}