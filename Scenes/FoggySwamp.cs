﻿using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class FoggySwamp : LAIScene
    {
        public override string SceneNameToken => "MAP_FOGGYSWAMP";
        public override string SeerToken => "BAZAAR_SEER_FOGGYSWAMP";
        public override GameObject BackgroundPrefab => LoadAsset("RoR2/Base/foggyswamp/FoggyswampDioramaDisplay.prefab");
        public override Vector3 Position => new(0f, -2.7f, 16);
        public override Quaternion Rotation => Quaternion.Euler(0, 30, 0);
        public override Vector3 Scale => new(1f, 0.5f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/goldshores/matSkyboxGoldshores.mat");
        public override string MusicTrackName => "muFULLSong06";
    }
}