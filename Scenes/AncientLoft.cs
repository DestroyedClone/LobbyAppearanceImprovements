using System;
using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class AncientLoft : LAIScene
    {
        public override string SceneNameToken => "MAP_ANCIENTLOFT";
        public override string SeerToken => "BAZAAR_SEER_ANCIENTLOFT";
        public override GameObject BackgroundPrefab => LoadAsset("RoR2/DLC1/ancientloft/AncientLoftDioramaDisplay.prefab");
        public override Vector3 Position => new Vector3(0f, -3.5f, 11);
        public override Quaternion Rotation => Quaternion.Euler(0f, 180f, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/DLC1/ancientloft/matSkyboxAncientLoft.mat");
        public override string MusicTrackName => "muGameplayDLC1_01";

    }
}