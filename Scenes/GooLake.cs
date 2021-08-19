using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class GooLake : LAIScene
    {
        public override string SceneName => "GooLake";
        public override GameObject BackgroundPrefab => Resources.Load<GameObject>("prefabs/stagedisplay/GoolakeDioramaDisplay");
        public override Vector3 Position => new Vector3(0, -0.5f, 8);
        public override Quaternion Rotation => new Quaternion();
        public override Vector3 Scale => new Vector3(0.25f, 0.1f, 0.25f);
    }
}