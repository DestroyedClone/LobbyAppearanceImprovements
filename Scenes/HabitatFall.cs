using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class HabitatFall : LAIScene 
    {
        public override string SceneNameToken => "MAP_HABITATFALL";
        public override string SeerToken => "BAZAAR_SEER_HABITATFALL";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new Vector3(2.5f, -2.7f, 20.5f);
        public override Quaternion Rotation => Quaternion.Euler(0, 330, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/DLC2/habitatfall/Assets/matBHFallSkybox.mat"); //todo get actual lobby mat

        public static GameObject display;
        public override string MusicTrackName => "muGameplayDLC2_01";

        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/DLC2/habitatfall/BHFallDioramaDisplay.prefab", "LAI_Scene_HabitatFall");
            //displayPrefab.transform.position = Position;
            //displayPrefab.transform.rotation = Rotation;
            //displayPrefab.transform.localScale = Scale;

            var skybox = CloneFromAddressable("RoR2/DLC2/habitatfall/Weather, HabitatFall.prefab");
            skybox.transform.parent = display.transform;
        }
    }
}