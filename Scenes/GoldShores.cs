using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class GoldShores : LAIScene
    {
        public override string SceneNameToken => "MAP_GOLDSHORES";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new Vector3(0, - 3.1f, 16);
        public override Quaternion Rotation => Quaternion.Euler(0, 30, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/goldshores/matSkyboxGoldshores.mat");
        public static GameObject display;
        //needs extra light
        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/Base/goldshores/GoldshoresDiorama.prefab", "LAI_Scene_GoldShores");

            var sky = CloneFromAddressable("RoR2/Base/goldshores/GoldshoresSkybox.prefab", display.transform);
        }
    }
}