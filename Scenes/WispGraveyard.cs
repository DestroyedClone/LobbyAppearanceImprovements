using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class WispGraveyard : LAIScene
    {
        public override string SceneNameToken => "MAP_WISPGRAVEYARD";
        public override string SeerToken => "BAZAAR_SEER_WISPGRAVEYARD";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new Vector3(0f, -10.7f, 15);
        public override Quaternion Rotation => Quaternion.Euler(0, 0, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/bazaar/matSkybox4.mat");
        public override string MusicTrackName => "muFULLSong18";
        public static GameObject display;

        //RoR2/Base/wispgraveyard/SkyboxDistantPlatforms.prefab

        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/Base/wispgraveyard/WispgraveyardDioramaDisplay.prefab", "LAI_Scene_WispGraveyard");

            var sky = CloneFromAddressable("RoR2/Base/wispgraveyard/WPSkybox.prefab", display.transform);
        }
    }
}