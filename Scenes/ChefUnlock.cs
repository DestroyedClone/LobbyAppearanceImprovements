using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class ChefUnlock : LAIScene 
    {
        public override string SceneNameToken => "CHEF_BODY_SUBTITLE";
        public override string SeerToken => "LAI_SEER_CHEFUNLOCK";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new Vector3(2.5f, -5.25f, 24.5f);
        public override Quaternion Rotation => Quaternion.Euler(0, 0, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        //public override Material SkyboxOverride => LoadAsset<Material>("chefLemTempleStuff"); //todo get actual lobby mat

        public static GameObject display;
        public override string MusicTrackName => "muGameplayDLC2_01";

        public override void Init()
        {
            base.Init();
            display = SceneSetup.chefLemTempleStuff;
            //displayPrefab.transform.position = Position;
            //displayPrefab.transform.rotation = Rotation;
            //displayPrefab.transform.localScale = Scale;

            var skybox = CloneFromAddressable("RoR2/DLC2/lakes/Weather, Lakes.prefab");
            skybox.transform.parent = display.transform;
        }
    }
}