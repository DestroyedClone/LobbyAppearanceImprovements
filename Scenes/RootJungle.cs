using UnityEngine;
using RoR2;

namespace LobbyAppearanceImprovements.Scenes
{
    public class RootJungle : LAIScene
    {
        public override string SceneNameToken => "MAP_ROOTJUNGLE";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new Vector3(0, -3.3f, 28.1f);
        public override Quaternion Rotation => Quaternion.Euler(0, 345, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/rootjungle/skyboxJungle.mat");
        public override string MusicTrackName => "muGameplayBase_09";
        //needs light
        public static GameObject display;
        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/Base/rootjungle/RootjungleDioramaDisplay.prefab", "LAI_Scene_RootJungle");

            var light = CloneFromAddressable("RoR2/Base/bazaar/BazaarLight1.prefab", display.transform);
            light.transform.Find("BlueFire").gameObject.SetActive(false);
            var plight = light.transform.Find("Point Light").gameObject;
            plight.GetComponent<FlickerLight>().enabled = false;
            //var clight = plight.GetComponent<Light>();
            //clight.color = new Color(1, 0.88f, 0, 1);
            light.transform.localPosition = new Vector3(-4.4258f, 8.5f, - 16.5173f);
            light.transform.localScale = Vector3.zero;
        }
    }
}