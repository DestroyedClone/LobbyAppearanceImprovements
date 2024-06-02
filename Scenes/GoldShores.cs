using RoR2;
using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class GoldShores : LAIScene
    {
        public override string SceneNameToken => "MAP_GOLDSHORES";
        public override string SeerToken => "BAZAAR_SEER_GOLDSHORES";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new Vector3(0, - 3.1f, 16);
        public override Quaternion Rotation => Quaternion.Euler(0, 30, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/goldshores/matSkyboxGoldshores.mat");
        public override string MusicTrackName => "muFULLSong02";
        public static GameObject display;
        //needs extra light
        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/Base/goldshores/GoldshoresDiorama.prefab", "LAI_Scene_GoldShores");

            var sky = CloneFromAddressable("RoR2/Base/goldshores/GoldshoresSkybox.prefab", display.transform);

            var light = CloneFromAddressable("RoR2/Base/bazaar/BazaarLight1.prefab", display.transform);
            light.transform.Find("BlueFire").gameObject.SetActive(false);
            var plight = light.transform.Find("Point Light").gameObject;
            plight.GetComponent<FlickerLight>().enabled = false;
            var clight = plight.GetComponent<Light>();
            clight.color = new Color(1, 0.88f, 0, 1);
            light.transform.localPosition = new Vector3(2.5f, 8.3f, -4.3301f);
            light.transform.localScale = Vector3.zero;
        }
    }
}