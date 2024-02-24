using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class RootJungle : LAIScene
    {
        public override string SceneNameToken => "MAP_ROOTJUNGLE";
        public override GameObject BackgroundPrefab => LoadAsset("RoR2/Base/rootjungle/RootjungleDioramaDisplay.prefab");
        public override Vector3 Position => new Vector3(0f, -5f, 30);
        public override Quaternion Rotation => Quaternion.Euler(0, 345, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
    }
}