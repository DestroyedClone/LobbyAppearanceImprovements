using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class GooLake : LAIScene
    {
        public override string SceneNameToken => "MAP_GOOLAKE";
        public override GameObject BackgroundPrefab => LoadAsset("RoR2/Base/goolake/GoolakeDioramaDisplay.prefab");
        public override Vector3 Position => new Vector3(0, -5f, 8);
        public override Quaternion Rotation => Quaternion.Euler(0, 0, 0);
        public override Vector3 Scale => Vector3.one;
    }
}