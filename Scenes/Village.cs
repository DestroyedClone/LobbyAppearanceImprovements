using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class Village : LAIScene 
    {
        public override string SceneNameToken => "MAP_VILLAGE_TITLE";
        public override string SeerToken => "BAZAAR_SEER_VILLAGE";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new(2.5f, -5.25f, 24.5f);
        public override Quaternion Rotation => Quaternion.Euler(0, 0, 0);
        public override Vector3 Scale => new(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/DLC2/village/Assets/matLVSkybox.mat"); //todo get actual lobby mat

        public static GameObject display;
        public override string MusicTrackName => "muGameplayDLC2_01";

        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/DLC2/village/LVDioramaDisplay.prefab", "LAI_Scene_Village");
            //displayPrefab.transform.position = Position;
            //displayPrefab.transform.rotation = Rotation;
            //displayPrefab.transform.localScale = Scale;

            var skybox = CloneFromAddressable("RoR2/DLC2/village/Weather, Village.prefab");
            skybox.transform.parent = display.transform;
        }
    }
}