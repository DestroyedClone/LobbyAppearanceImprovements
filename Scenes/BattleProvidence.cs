using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class CaptainsHelm : LAIScene
    {
        public override string SceneNameToken => "LAI_MAP_CAPTAINSHELM";
        public override string SeerToken => "LAI_SEER_CAPTAINSHELM";
        public override GameObject BackgroundPrefab => SceneSetup.CaptainHelmObject;
        public override Vector3 Position => new Vector3(-16.7f, -3.7f, 8);
        public override Quaternion Rotation => Quaternion.Euler(0f, 180f, 0);//Quaternion.Euler(10f, 179f, 0);
        public override Vector3 Scale => Vector3.one;
        public override string PreferredLayout => nameof(Layouts.CaptainsHelm_Default);
        //public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/intro/matIntroSkybox.mat");
        public override string MusicTrackName => "muSong22";
        public static GameObject display;

        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/Base/arena/ArenaDioramaDisplay.prefab", "LAI_Scene_Arena");
            //displayPrefab.transform.position = Position;
            //displayPrefab.transform.rotation = Rotation;
            //displayPrefab.transform.localScale = Scale;
            var gem = CloneFromAddressable("RoR2/Base/arena/Arena_NullifierGemProp.prefab", display.transform);
            gem.transform.position = new Vector3(3.9f, 0.2f, 8.3f);
            gem.transform.localScale = Vector3.one * 2;
            //var portal = CloneFromAddressable("RoR2/Base/PortalArena/PortalArena.prefab", display.transform);
            //portal.transform.position = new Vector3(9, 0, 28);

            var portal = display.AddComponent<PrefabSpawner>();
            portal.AssetPath = "RoR2/Base/PortalArena/PortalArena.prefab";
            portal.localPosition = new Vector3(9, 0, 28);
            portal.enabled = false;

            var skybox = CloneFromAddressable("RoR2/Base/arena/Arena_Skybox.prefab");
            skybox.transform.parent = display.transform;
        }
    }
}