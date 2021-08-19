using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class FoggySwamp : LAIScene
    {
        public override string SceneName => "FoggySwamp";
        public override GameObject BackgroundPrefab => Resources.Load<GameObject>("prefabs/stagedisplay/FoggyswampDioramaDisplay");
        public override Vector3 Position => new Vector3(0f, -2.7f, 16);
        public override Quaternion Rotation => Quaternion.Euler(0, 30, 0);
        public override Vector3 Scale => new Vector3(1f, 0.5f, 1f);
    }
}