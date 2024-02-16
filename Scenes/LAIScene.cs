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
        public virtual GameObject TitleInstance { get; set; }
        public virtual GameObject SubTitleInstance { get; set; }
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
            CreateTitleText();
            return sceneInstance;
        }

        //yeesh
        private class TitleRefEnsurer : MonoBehaviour
        {
            public bool foundRef = false;
            public float stopwatch;
            private const float cooldown = 3f;

            private void Awake()
            {
                stopwatch = cooldown;
            }

            private void FixedUpdate()
            {
                stopwatch -= Time.fixedDeltaTime;
                if (stopwatch < 0)
                {
                    stopwatch = cooldown;
                    var refCheck = LAIPlugin.CharacterSelectController.transform.Find("SurvivorNamePanel/SurvivorName");
                    if (refCheck)
                        LAIPlugin.LAITitleRef = refCheck;
                    if (LAIPlugin.LAITitleRef)
                    {
                        LAILogging.LogMessage($"Found the titleref!", LoggingStyle.UserShouldSee);
                        LAISceneManager.chosenScene.CreateTitleText();
                        enabled = false;
                        Destroy(gameObject);
                    }
                }
            }
        }

        public void CreateTitleText()
        {
            //if (!LAIPlugin.LAITitleRef) LAIPlugin.LAITitleRef = LAIPlugin.characterSelectController.transform.Find("SurvivorNamePanel/SurvivorName");
            if (!LAIPlugin.LAITitleRef)
            {
                LAILogging.LogMessage($"LAITITLEREF missing, attempting spawn", LoggingStyle.UserShouldSee);
                var obj = new GameObject();
                obj.name = $"LAITITLEREFGAMEOBJECT";
                obj.AddComponent<TitleRefEnsurer>();
                return;
            }
            LAILogging.LogMessage($"LAITITLEREF existing!, attempting spawn", LoggingStyle.UserShouldSee);
            TitleInstance = UnityEngine.Object.Instantiate(LAIPlugin.LAITitleRef.gameObject, LAIPlugin.CharacterSelectController.transform);
            TitleInstance.name = $"LobbyAppearanceImprovements_Scene_Title";
            SubTitleInstance = UnityEngine.Object.Instantiate(LAIPlugin.LAITitleRef.gameObject, LAIPlugin.CharacterSelectController.transform);
            SubTitleInstance.name = $"LobbyAppearanceImprovements_Scene_Subtitle";

            TitleInstance.transform.localPosition = new Vector3(0f, 450f, 0f);
            SubTitleInstance.transform.localPosition = new Vector3(0f, 500f, 300f);

            TitleInstance.GetComponent<HGTextMeshProUGUI>().text = RoR2.Language.GetString(GetTitleToken());
            //TitleInstance.GetComponent<HGTextMeshProUGUI>().alpha = 0f;
            //TitleInstance.GetComponent<HGTextMeshProUGUI>().CrossFadeAlpha(1f, 3f, false);

            SubTitleInstance.GetComponent<HGTextMeshProUGUI>().text = $"<color=grey>{RoR2.Language.GetString(GetSubtitleToken())}</color>";
            //SubTitleInstance.GetComponent<HGTextMeshProUGUI>().alpha = 0f;
            //SubTitleInstance.GetComponent<HGTextMeshProUGUI>().CrossFadeAlpha(1f, 3f, false);
            HookMethods.Hook_ToggleSceneHeaderVisibility(ConfigSetup.Scene_Header.Value);
        }

        public void OnDestroy()
        {
            if (TitleInstance) UnityEngine.Object.Destroy(TitleInstance);
            if (SubTitleInstance) UnityEngine.Object.Destroy(SubTitleInstance);
            onSceneUnloaded?.Invoke(this);
        }

        public static GameObject LoadAsset(string path)
        {
            return Addressables.LoadAssetAsync<GameObject>(path).WaitForCompletion();
        }

        public string GetTitleToken()
        {
            return SceneNameToken + "_TITLE";
        }

        public string GetSubtitleToken()
        {
            return SceneNameToken + "_SUBTITLE";
        }
    }
}