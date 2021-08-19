﻿using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class RootJungle : LAIScene
    {
        public override string SceneName => "RootJungle";
        public override GameObject BackgroundPrefab => Resources.Load<GameObject>("prefabs/stagedisplay/RootjungleDioramaDisplay");
        public override Vector3 Position => new Vector3(0f, -5f, 30);
        public override Quaternion Rotation => Quaternion.Euler(0, 345, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
    }
}