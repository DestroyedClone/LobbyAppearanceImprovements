using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class SkyMeadow : LAIScene
    {
        public override string SceneNameToken => "MAP_SKYMEADOW";
        public override string SeerToken => "BAZAAR_SEER_SKYMEADOW";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new Vector3(0f, -3f, 0);
        public override Quaternion Rotation => Quaternion.Euler(0, 0, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/skymeadow/matSMSkybox.mat");
        public override string MusicTrackName => "muSong14";

        //RoR2/Base/skymeadow/matSMSkybox.mat
        //RoR2/Base/skymeadow/matSMSkyboxMoon.mat
        public static GameObject display;

        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/Base/skymeadow/SkyMeadowDioramaDisplay.prefab", "LAI_Scene_SkyMeadow");

            var sky = CloneFromAddressable("RoR2/Base/skymeadow/SMSkyboxPrefab.prefab", display.transform);
        }
    }
}