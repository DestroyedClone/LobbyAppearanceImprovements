using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class GooLake : LAIScene
    {
        public override string SceneName => "GooLake";
        public override GameObject BackgroundPrefab => Resources.Load<GameObject>("prefabs/stagedisplay/GoolakeDioramaDisplay");
        public override Vector3 Position => new Vector3();
        public override Vector3 Rotation => new Vector3();
    }
}