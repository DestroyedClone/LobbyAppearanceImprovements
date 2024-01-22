﻿using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LobbyAppearanceImprovements.Scenes
{
    public class AncientLoft : LAIScene
    {
        public override string SceneName => "Aphelian Sanctuary";
        public override string SceneNameToken => "MAP_ANCIENTLOFT";
        public override GameObject BackgroundPrefab => LoadAsset("RoR2/DLC1/ancientloft/AncientLoftDioramaDisplay.prefab");
        public override Vector3 Position => new Vector3(3.5f, -1.5f, 3);
        public override Quaternion Rotation => Quaternion.Euler(0f, 0f, 0);
        public override Vector3 Scale => new Vector3(1f, 0.5f, 1f);
    }
}