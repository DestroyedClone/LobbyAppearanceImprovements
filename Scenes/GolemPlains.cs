using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class GolemPlains : LAIScene
    {
        public override string SceneNameToken => "MAP_GOLEMPLAINS";
        public override GameObject BackgroundPrefab => LoadAsset("RoR2/Base/golemplains/GolemplainsDioramaDisplay.prefab");
        public override Vector3 Position => new Vector3(9f, -3.1f, 9);
        public override Quaternion Rotation => Quaternion.Euler(0, 180, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/golemplains/matSkyboxGolemplainsFoggy.mat");
    }
}