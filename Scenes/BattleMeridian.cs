using RoR2;
using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class BattleMeridian : LAIScene
    {
        public override string SceneNameToken => "LAI_MAP_BATTLEMERIDIAN";
        public override string SeerToken => "LAI_SEER_BATTLEMERIDIAN";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new(0f, 0f, 0);
        public override Quaternion Rotation => Quaternion.Euler(0f, 0f, 0);//Quaternion.Euler(10f, 179f, 0);
        public override Vector3 Scale => Vector3.one;
        public override string PreferredLayout => nameof(Layouts.CaptainsHelm_Default);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/DLC2/meridian/matSkyboxPMActivate.mat");
        public override string MusicTrackName => "muSong_MeridianFSB";

        public static GameObject display;

        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/DLC2/meridian/Assets/PMCake.prefab", "LAI_Scene_Arena");
            var gem = CloneFromAddressable("RoR2/DLC2/FalseSon/FalseSonDisplay.prefab", display.transform);
            gem.transform.position = new Vector3(0f, 0f, 0f);
            gem.transform.localScale = Vector3.one;
        }
    }
}