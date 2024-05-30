using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class Lakes : LAIScene
    {
        public override string SceneNameToken => "MAP_LAKES";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new Vector3(2.5f, -5.25f, 24.5f);
        public override Quaternion Rotation => Quaternion.Euler(0, 0, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/CU8/lakes/matTLSkybox.mat"); //todo get actual lobby mat

        public static GameObject display;
        public override string MusicTrackName => "muGameplayDLC2_01";

        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/CU8/lakes/TLDioramaDisplay.prefab", "LAI_Scene_Lakes");
            //displayPrefab.transform.position = Position;
            //displayPrefab.transform.rotation = Rotation;
            //displayPrefab.transform.localScale = Scale;

            var skybox = CloneFromAddressable("RoR2/CU8/lakes/Weather, Lakes.prefab");
            skybox.transform.parent = display.transform;
        }
    }
}