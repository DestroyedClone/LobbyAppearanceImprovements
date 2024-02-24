using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class FrozenWall : LAIScene
    {
        public override string SceneNameToken => "MAP_FROZENWALL";
        public override GameObject BackgroundPrefab => LoadAsset("RoR2/Base/frozenwall/FrozenwallDioramaDisplay.prefab");
        public override Vector3 Position => new Vector3(7f, -2.25f, 7);
        public override Quaternion Rotation => Quaternion.Euler(0, 260, 0);
        public override Vector3 Scale => new Vector3(.5f, 0.5f, .5f);
    }
}