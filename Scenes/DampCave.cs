using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class DampCave : LAIScene
    {
        public override string SceneNameToken => "MAP_DAMPCAVE";
        public override GameObject BackgroundPrefab => LoadAsset("RoR2/Base/dampcavesimple/DampcaveDioramaDisplay.prefab");
        public override Vector3 Position => new Vector3(4f, -2f, 4);
        public override Quaternion Rotation => Quaternion.Euler(0, 60, 0);
        public override Vector3 Scale => new Vector3(1f, 0.5f, 1f);
    }
}