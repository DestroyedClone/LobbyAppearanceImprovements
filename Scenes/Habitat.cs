using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class Habitat : LAIScene 
    {
        public override string SceneNameToken => "MAP_HABITAT";
        public override string SeerToken => "BAZAAR_SEER_HABITAT";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new Vector3(2.5f, - 2.7f, 20.5f);
        public override Quaternion Rotation => Quaternion.Euler(0, 330, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/DLC2/habitat/Assets/matBHSkybox.mat");

        public static GameObject display;
        public override string MusicTrackName => "muGameplayDLC2_01";

        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/DLC2/habitat/BHDioramaDisplay.prefab", "LAI_Scene_Habitat");
            //displayPrefab.transform.position = Position;
            //displayPrefab.transform.rotation = Rotation;
            //displayPrefab.transform.localScale = Scale;

            var skybox = CloneFromAddressable("RoR2/DLC2/habitat/Weather, Habitat.prefab");
            skybox.transform.parent = display.transform;
        }
    }
}