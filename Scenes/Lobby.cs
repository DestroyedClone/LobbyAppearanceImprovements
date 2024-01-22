﻿using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class Lobby : LAIScene
    {
        public override string SceneName => "Lobby";
        public override string SceneNameToken => "LAI_MAP_LOBBY";
        public override GameObject BackgroundPrefab => LoadAsset("RoR2/Base/Common/DefaultLobbyBackground.prefab");
        public override Vector3 Position => Vector3.zero;
        public override Quaternion Rotation => Quaternion.identity;
        public override Vector3 Scale => Vector3.one;
    }
}