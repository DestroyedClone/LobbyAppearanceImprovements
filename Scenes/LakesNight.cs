using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class LakesNight : LAIScene
    {
        public override string SceneNameToken => "MAP_LAKESNIGHT_TITLE";
        public override string SeerToken => "BAZAAR_SEER_LAKESNIGHT";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new Vector3(2.5f, -5.25f, 24.5f);
        public override Quaternion Rotation => Quaternion.Euler(0, 0, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/DLC2/lakesnight/Assets/matTLNightSkybox.mat"); //todo get actual lobby mat

        public static GameObject display;
        public override string MusicTrackName => "muGameplayDLC2_01";

        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/DLC2/lakesnight/TLNightDioramaDisplay.prefab", "LAI_Scene_LakesNight");
            //displayPrefab.transform.position = Position;
            //displayPrefab.transform.rotation = Rotation;
            //displayPrefab.transform.localScale = Scale;

            var skybox = CloneFromAddressable("RoR2/DLC2/lakesnight/Weather, Lakesnight.prefab");
            skybox.transform.parent = display.transform;
        }
    }
}