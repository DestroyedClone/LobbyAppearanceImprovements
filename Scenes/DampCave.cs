﻿using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class DampCave : LAIScene
    {
        public override string SceneNameToken => "MAP_DAMPCAVE";
        public override string SeerToken => "BAZAAR_SEER_DAMPCAVESIMPLE";
        public override GameObject BackgroundPrefab => LoadAsset("RoR2/Base/dampcavesimple/DampcaveDioramaDisplay.prefab");
        public override Vector3 Position => new(4f, -1.6f, 4);
        public override Quaternion Rotation => Quaternion.Euler(0, 60, 0);
        public override Vector3 Scale => new(1f, 0.5f, 1f);

        //public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/dampcave/matDCTerrainGiantColumns.mat");
        public override string MusicTrackName => "muFULLSong19";
    }
}