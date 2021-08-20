using UnityEngine;
using static LobbyAppearanceImprovements.Methods;

namespace LobbyAppearanceImprovements.CharacterSceneSetups
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

        public abstract CharacterLayout[] CharacterLayouts { get; }

        public struct CharacterLayout
        {
            public string BodyName;
            public Vector3 Position;
            public Vector3 Rotation;
        }

        public GameObject CreateLayout()
        {
            var layoutHolder = new GameObject();
            foreach (var characterLayout in CharacterLayouts)
            {
                CreateDisplay(characterLayout.BodyName, characterLayout.Position, characterLayout.Rotation, layoutHolder.transform);
            }
            return layoutHolder;
        }
    }
}