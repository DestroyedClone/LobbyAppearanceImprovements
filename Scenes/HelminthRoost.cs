using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class HelminthRoost : LAIScene 
    {
        public override string SceneNameToken => "MAP_HELMINTHROOST";
        public override string SeerToken => "BAZAAR_SEER_HELMINTH";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new(2.5f, -5.25f, 24.5f);
        public override Quaternion Rotation => Quaternion.Euler(0f, 120f, 0);
        public override Vector3 Scale => new(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/DLC2/helminthroost/Assets/matSkyboxHelminthGrey.mat"); //todo get actual lobby mat

        public static GameObject display;
        public override string MusicTrackName => "muGameplayDLC2_01";

        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/DLC2/helminthroost/HRDioramaDisplay.prefab", "LAI_Scene_HelminthRoost");
            //displayPrefab.transform.position = Position;
            //displayPrefab.transform.rotation = Rotation;
            //displayPrefab.transform.localScale = Scale;

            var skybox = CloneFromAddressable("RoR2/DLC2/helminthroost/Weather, Helminthroost.prefab");
            skybox.transform.parent = display.transform;
        }
    }
}