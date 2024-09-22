using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class GolemPlains : LAIScene
    {
        public override string SceneNameToken => "MAP_GOLEMPLAINS";
        public override string SeerToken => "BAZAAR_SEER_GOLEMPLAINS";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new Vector3(4f, -3f, 24);
        public override Quaternion Rotation => Quaternion.Euler(0, 240, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/golemplains/matSkyboxGolemplainsFoggy.mat");
        public override string MusicTrackName => "muFULLSong18";
        public static GameObject display;

        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/Base/golemplains/GolemplainsDioramaDisplay.prefab", "LAI_Scene_GolemPlains");

            //var sky = CloneFromAddressable("RoR2/Base/goldshores/GoldshoresSkybox.prefab", display.transform);

            var light = CloneFromAddressable("RoR2/Base/bazaar/Bazaar_Light.prefab", display.transform);
            light.transform.Find("FireLODLevel").gameObject.SetActive(false);
            var plight = light.transform.Find("Point Light").gameObject;
            plight.GetComponent<RoR2.FlickerLight>().enabled = false;
            var clight = plight.GetComponent<Light>();
            clight.color = new Color(0.76f, 0.7925f, 0, 1);
            light.transform.localPosition = new Vector3(1f, 6.5f, 21f);
            light.transform.localScale = Vector3.zero;
        }
    }
}