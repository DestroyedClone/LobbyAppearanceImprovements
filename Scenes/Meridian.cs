using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class Meridian : LAIScene 
    {
        public override string SceneNameToken => "MAP_MERIDIAN";
        public override string SeerToken => "BAZAAR_SEER_MERIDIAN";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new(2.5f, -5.25f, 24.5f);
        public override Quaternion Rotation => Quaternion.Euler(0, 0, 0);
        public override Vector3 Scale => new (1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/DLC2/meridian/matSkyboxPM.mat"); //todo get actual lobby mat
        //RoR2/DLC2/meridian/matSkyboxPM.mat
        //RoR2/DLC2/meridian/matSkyboxPMActivate.mat

        public static GameObject display;
        public override string MusicTrackName => "muGameplayDLC2_01";

        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/DLC2/meridian/PMDioramaDisplay.prefab", "LAI_Scene_Meridian");
            //displayPrefab.transform.position = Position;
            //displayPrefab.transform.rotation = Rotation;
            //displayPrefab.transform.localScale = Scale;

            var skybox = CloneFromAddressable("RoR2/DLC2/meridian/Weather, Meridian.prefab");
            skybox.transform.parent = display.transform;

            var head = CloneFromAddressable("RoR2/DLC2/meridian/Assets/PMHead.prefab");
            head.transform.parent = display.transform;
            head.transform.localScale = Vector3.one * 0.05f;
            head.transform.rotation = Quaternion.Euler(330, 180, 0);
            head.transform.localPosition = new Vector3(-3, 4, 12);
            head.transform.Find("Point Light").gameObject.SetActive(false);
            head.transform.Find("meshPMEyesGlow").gameObject.SetActive(false);
            head.transform.Find("meshPMCrownAuraBeams").gameObject.SetActive(false);
            head.transform.Find("meshPMCrownGlow").gameObject.SetActive(false);
            head.transform.Find("ColossusGoldEnergyDecal").gameObject.SetActive(false);
        }
    }
}