using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class BlackBeach : LAIScene
    {
        public override string SceneNameToken => "MAP_BLACKBEACH";
        public override string SeerToken => "BAZAAR_SEER_BLACKBEACH";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new Vector3(0f, -2.4f, 5);
        public override Quaternion Rotation => new Quaternion();
        public override Vector3 Scale => new Vector3(1f, 0.5f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/Common/Skyboxes/matSkyboxFoggy.mat");
        public override string MusicTrackName => "muFULLSong07";

        public static GameObject display;

        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/Base/blackbeach/BlackbeachDioramaDisplay.prefab", "LAI_Scene_BlackBeach");

            var sky = CloneFromAddressable("RoR2/Base/blackbeach/BBSkybox.prefab", display.transform);

            var light = CloneFromAddressable("RoR2/Base/bazaar/BazaarLight1.prefab", display.transform);
            light.transform.Find("BlueFire").gameObject.SetActive(false);
            var plight = light.transform.Find("Point Light").gameObject;
            plight.GetComponent<RoR2.FlickerLight>().enabled = false;
            var clight = plight.GetComponent<Light>();
            clight.color = new Color(0.6306f, 0.5548f, 0.82f, 1);
            clight.intensity = 200;
            light.transform.localPosition = new Vector3(2f, 16.8f, 9);
            light.transform.localScale = Vector3.zero;
        }
    }
}