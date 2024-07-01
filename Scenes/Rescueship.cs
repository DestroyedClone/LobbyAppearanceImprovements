using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class RescueShip : LAIScene
    {
        public override string SceneNameToken => "LAI_MAP_RESCUESHIP";
        public override string SeerToken => "LAI_SEER_RESCUESHIP";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new Vector3(-1, 5.5f, 35.1f);
        public override Quaternion Rotation => Quaternion.Euler(0f, 0, 0);
        public override Vector3 Scale => new Vector3(1, 1f, 1);
        public override string MusicTrackName => "muSong24";

        //public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/moon2/matSkyboxMoon.mat");
        public static GameObject display;

        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/Base/moon2/RescueshipMoon.prefab", "LAI_Scene_RescueShip");
            display.transform.Find("escapeship").transform.localPosition = Vector3.zero;

            //var ship2 = CloneFromAddressable("RoR2/Base/outro/RescueshipOutroCutscene.prefab", display.transform);
        }
    }
}