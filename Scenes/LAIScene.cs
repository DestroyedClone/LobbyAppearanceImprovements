using RoR2.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LobbyAppearanceImprovements.Scenes
{
    public abstract class LAIScene
    {
        public LAIScene()
        {
        }

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
                sceneInstance.transform.position = Position;
                sceneInstance.transform.rotation = Rotation;
                sceneInstance.transform.localScale = Scale;
            }
            return sceneInstance;
        }

        public void CreateTitleText()
        {
            var textInstance = new GameObject();

            TitleInstance = textInstance;
            if (!LAIPlugin.TitleRef) return;
            if (!LAIPlugin.characterSelectController) return;
            TitleInstance = UnityEngine.Object.Instantiate(LAIPlugin.TitleRef.gameObject, LAIPlugin.characterSelectController.transform);
            SubTitleInstance = UnityEngine.Object.Instantiate(LAIPlugin.TitleRef.gameObject, LAIPlugin.characterSelectController.transform);

            TitleInstance.transform.localPosition = new Vector3(0f, 450f, 0f);
            SubTitleInstance.transform.localPosition = new Vector3(0f, 500f, 300f);

            TitleInstance.GetComponent<HGTextMeshProUGUI>().text = RoR2.Language.GetString(GetTitleToken());
            SubTitleInstance.GetComponent<HGTextMeshProUGUI>().text = $"<color=grey>{RoR2.Language.GetString(GetSubtitleToken())}</color>";
        }

        public void OnDestroy()
        {
            if (TitleInstance) Object.Destroy(TitleInstance);
            if (SubTitleInstance) Object.Destroy(SubTitleInstance);
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