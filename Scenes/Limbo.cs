using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class Limbo : LAIScene
    {
        public override string SceneNameToken => "MAP_LIMBO";
        public override GameObject BackgroundPrefab => LoadAsset("RoR2/Base/limbo/LimboDioramaDisplay.prefab");
        public override Vector3 Position => new Vector3(0f, -3.05f, 0);
        public override Quaternion Rotation => Quaternion.Euler(0, 0, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/mysteryspace/matSkyboxMysterySpace.mat"); //todo get actual lobby mat
    }
}