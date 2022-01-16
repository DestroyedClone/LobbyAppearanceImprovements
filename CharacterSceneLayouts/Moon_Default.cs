using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;

namespace LobbyAppearanceImprovements.CharacterSceneLayouts
{
    public class Moon_Default : CharSceneLayout
    {
        public override string SceneLayout => "Moon_Default";
        public override string SceneName => "Moon";
        public override string Author => "DestroyedClone";
        public override string LayoutName => "Default";
        public override Dictionary<string, Vector3[]> CharacterLayouts => new Dictionary<string, Vector3[]>()
        {
            { "Commando", new [] {new Vector3(2.65f, 0.01f, 6.00f), new Vector3(0f, 240f, 0f) } },
        };
    }
}
