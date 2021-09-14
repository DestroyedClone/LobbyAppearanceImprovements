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
        public virtual GameObject TitleInstance { get; set; }
        public virtual string PreferredLayout { get; } 

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
        }

        public void OnDestroy()
        {
            if (TitleInstance)
            {
                Object.Destroy(TitleInstance);
            }
        }


    }
}