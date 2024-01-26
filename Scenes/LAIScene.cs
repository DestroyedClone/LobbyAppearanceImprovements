using RoR2.UI;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
        public virtual string[] RequiredModGUID { get; }

        public GameObject CreateScene()
        {
            CreateTitleText();
            GameObject sceneInstance = null;
            if (BackgroundPrefab)
            {
                sceneInstance = UnityEngine.Object.Instantiate<GameObject>(BackgroundPrefab);
                sceneInstance.transform.SetPositionAndRotation(Position, Rotation);
                sceneInstance.transform.localScale = Scale;
            }
            onSceneLoaded?.Invoke(this);
            return sceneInstance;
        }

        public void CreateTitleText()
        {
            if (!LAIPlugin.characterSelectController) return;
            if (!LAIPlugin.TitleRef) LAIPlugin.TitleRef = LAIPlugin.characterSelectController.transform.Find("SurvivorNamePanel/SurvivorName");
            if (!LAIPlugin.TitleRef) return;
            TitleInstance = UnityEngine.Object.Instantiate(LAIPlugin.TitleRef.gameObject, LAIPlugin.characterSelectController.transform);
            TitleInstance.name = $"LobbyAppearanceImprovements_Scene_Title";
            SubTitleInstance = UnityEngine.Object.Instantiate(LAIPlugin.TitleRef.gameObject, LAIPlugin.characterSelectController.transform);
            SubTitleInstance.name = $"LobbyAppearanceImprovements_Scene_Subtitle";

            TitleInstance.transform.localPosition = new Vector3(0f, 450f, 0f);
            SubTitleInstance.transform.localPosition = new Vector3(0f, 500f, 300f);

            TitleInstance.GetComponent<HGTextMeshProUGUI>().text = RoR2.Language.GetString(GetTitleToken());
            SubTitleInstance.GetComponent<HGTextMeshProUGUI>().text = $"<color=grey>{RoR2.Language.GetString(GetSubtitleToken())}</color>";
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