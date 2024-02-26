using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class BlackBeach : LAIScene
    {
        public override string SceneNameToken => "MAP_BLACKBEACH";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new Vector3(0f, -2.4f, 5);
        public override Quaternion Rotation => new Quaternion();
        public override Vector3 Scale => new Vector3(1f, 0.5f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/Common/Skyboxes/matSkyboxFoggy.mat");

        public static GameObject display;

        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/Base/blackbeach/BlackbeachDioramaDisplay.prefab", "LAI_Scene_BlackBeach");

            var sky = CloneFromAddressable("RoR2/Base/blackbeach/BBSkybox.prefab", display.transform);
        }
    }
}