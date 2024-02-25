using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class BlackBeach : LAIScene
    {
        public override string SceneNameToken => "MAP_BLACKBEACH";
        public override GameObject BackgroundPrefab => LoadAsset("RoR2/Base/blackbeach/BlackbeachDioramaDisplay.prefab");
        public override Vector3 Position => new Vector3(0f, -2.4f, 5);
        public override Quaternion Rotation => new Quaternion();
        public override Vector3 Scale => new Vector3(1f, 0.5f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/Common/Skyboxes/matSkyboxFoggy.mat");
    }
}