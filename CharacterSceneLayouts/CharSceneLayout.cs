using System.Collections.Generic;
using UnityEngine;
using static LobbyAppearanceImprovements.Methods;

namespace LobbyAppearanceImprovements.CharacterSceneLayouts
{
    public abstract class CharSceneLayout
    {
        public CharSceneLayout()
        {
        }

        // Technical Name of the scene
        public abstract string SceneLayout { get; }

        // Name of the Scene : "Lobby"
        public abstract string SceneName { get; }

        // Author of the scene : "DestroyedClone"
        public abstract string Author { get; }

        // Name of the layout : "Original Crew"
        public abstract string LayoutName { get; }

        public virtual string[] RequiredModGUID { get; }

        public abstract Dictionary<string, Vector3[]> CharacterLayouts { get; }

        public bool HasSetup = false;

        public virtual Dictionary<string, CameraSetting> CharacterCameraSettings { get; }

        public struct CameraSetting
        {
            public CameraSetting(float Fov = 60, float Pitch = 0, float Yaw = 0, Vector3 Position = new Vector3(), Vector3 Rotation = new Vector3())
            {
                fov = Fov;
                pitch = Pitch;
                yaw = Yaw;
                rotation = Rotation;
                position = Position;
            }

            public float fov { get; }
            public float pitch { get; }
            public float yaw { get; }
            public Vector3 position { get; }
            public Vector3 rotation { get; }
        }

        public GameObject CreateLayout()
        {
            var layoutHolder = new GameObject();
            layoutHolder.name = "HOLDER: LAYOUT (" + SceneLayout + ")";
            foreach (var characterLayout in CharacterLayouts)
            {
                CreateDisplay(characterLayout.Key, characterLayout.Value[0], characterLayout.Value[1], layoutHolder.transform);
            }
            foreach (var obj in CreateAdditionalObjectsOnLoad())
            {
                obj.transform.parent = layoutHolder.transform;
            }

            return layoutHolder;
        }

        public virtual List<GameObject> CreateAdditionalObjectsOnLoad()
        {
            return new List<GameObject>();
        }

        // For creating objects at runtime
        public virtual void Init()
        {
            if (ConfigSetup.ShowLoggingText.Value > ConfigSetup.LoggingStyle.None)
                LAIPlugin._logger.LogMessage($"{SceneLayout}.Init :: Setting up layout.");
            if (HasSetup)
            {
                if (ConfigSetup.ShowLoggingText.Value > ConfigSetup.LoggingStyle.None)
                    LAIPlugin._logger.LogMessage($"{SceneLayout}.Init :: Ran Init(), but has already set up!");
                return;
            }
            HasSetup = true;
        }
    }
}