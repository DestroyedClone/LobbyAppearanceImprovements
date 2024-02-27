using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class GolemPlains : LAIScene
    {
        public override string SceneNameToken => "MAP_GOLEMPLAINS";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new Vector3(9f, - 3.5f, 9);
        public override Quaternion Rotation => Quaternion.Euler(0, 240, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/golemplains/matSkyboxGolemplainsFoggy.mat");
        public static GameObject display;
        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/Base/golemplains/GolemplainsDioramaDisplay.prefab", "LAI_Scene_GolemPlains");

            //var sky = CloneFromAddressable("RoR2/Base/goldshores/GoldshoresSkybox.prefab", display.transform);

            var light = CloneFromAddressable("RoR2/Base/bazaar/BazaarLight1.prefab", display.transform);
            light.transform.Find("BlueFire").gameObject.SetActive(false);
            var plight = light.transform.Find("Point Light").gameObject;
            plight.GetComponent<RoR2.FlickerLight>().enabled = false;
            var clight = plight.GetComponent<Light>();
            clight.color = new Color(0.76f, 0.7925f, 0, 1);
            light.transform.localPosition = new Vector3(3.634f, 6.5f, 8.2942f);
            light.transform.localScale = Vector3.zero;
        }
    }
}