using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public abstract class LAIScene
    {
        public LAIScene()
        {
        }

        public abstract string SceneName { get; }
        public abstract GameObject BackgroundPrefab { get; }
        public abstract Vector3 Position { get; }
        public abstract Quaternion Rotation { get; }
        public abstract Vector3 Scale { get; }
        public virtual GameObject titleInstance { get; set; }

        public GameObject CreateScene()
        {
            CreateTitleText();
            var sceneInstance = UnityEngine.Object.Instantiate<GameObject>(BackgroundPrefab);
            sceneInstance.transform.position = Position;
            sceneInstance.transform.rotation = Rotation;
            sceneInstance.transform.localScale = Scale;
            return sceneInstance;
        }

        public void CreateTitleText()
        {
            var textInstance = new GameObject();

            titleInstance = textInstance;
        }

        public void OnDestroy()
        {
            if (titleInstance)
            {
                Object.Destroy(titleInstance);
            }
        }
    }
}