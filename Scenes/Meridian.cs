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
            var childLoc = display.AddComponent<ChildLocator>();

            var skybox = CloneFromAddressable("RoR2/DLC2/meridian/Weather, Meridian.prefab");
            skybox.transform.parent = display.transform;

            var head = CloneFromAddressable("RoR2/DLC2/meridian/Assets/PMHead.prefab");
            head.transform.parent = display.transform;
            head.transform.localScale = Vector3.one * 0.05f;
            head.transform.rotation = Quaternion.Euler(330, 180, 0);
            head.transform.localPosition = new Vector3(-3, 4, 12);
            head.transform.Find("Point Light").gameObject.SetActive(false);

            Transform eyeGlow = head.transform.Find("meshPMEyesGlow");
            eyeGlow.gameObject.SetActive(false);
            Transform crownAuraBeams = head.transform.Find("meshPMCrownAuraBeams");
            crownAuraBeams.gameObject.SetActive(false);
            Transform crownGlow = head.transform.Find("meshPMCrownGlow");
            crownGlow.gameObject.SetActive(false);
            Transform goldDecal = head.transform.Find("ColossusGoldEnergyDecal");
            goldDecal.gameObject.SetActive(false);

            childLoc.transformPairs = new ChildLocator.NameTransformPair[]
            {
                new ChildLocator.NameTransformPair() {name = "eyeGlow", transform = eyeGlow},
                new ChildLocator.NameTransformPair() {name = "crownAuraBeams", transform = crownAuraBeams},
                new ChildLocator.NameTransformPair() {name = "crownGlow", transform = crownGlow},
                new ChildLocator.NameTransformPair() {name = "goldDecal", transform = goldDecal},
            };
        }

        public override void OnVoteStarted(LAIScene scene)
        {
            base.OnVoteStarted(scene);
            if (!scene.IsSceneOfType<Meridian>()) return;
            //var sceneInstance = LAISceneManager.sceneInstance;
            if (!LAISceneManager.sceneInstance) return;
            var childLoc = LAISceneManager.sceneInstance.GetComponent<ChildLocator>();
            foreach (var child in childLoc.transformPairs)
            {
                child.transform.gameObject.SetActive(true);
            }
        }
    }
}