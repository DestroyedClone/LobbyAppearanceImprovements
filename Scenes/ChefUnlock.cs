using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class ChefUnlock : LAIScene 
    {
        public override string SceneNameToken => "CHEF_BODY";
        public override string SeerToken => "LAI_SEER_CHEFUNLOCK";
        public override GameObject BackgroundPrefab => SceneSetup.chefLemTempleStuff;
        public override Vector3 Position => new (41f, 51.5f, - 166);
        public override Quaternion Rotation => Quaternion.Euler(0, 210, 0);
        public override Vector3 Scale => new (1f, 1f, 1f);
        //public override Material SkyboxOverride => LoadAsset<Material>("chefLemTempleStuff"); //todo get actual lobby mat
        public override string MusicTrackName => "muGameplayDLC2_01";
    }
}