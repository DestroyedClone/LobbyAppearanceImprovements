using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public abstract class LAIScene
    {
        public abstract string SceneName { get; }
        public abstract GameObject BackgroundPrefab { get; }
        public abstract Vector3 Position { get; }
        public abstract Vector3 Rotation { get; }
    }
}