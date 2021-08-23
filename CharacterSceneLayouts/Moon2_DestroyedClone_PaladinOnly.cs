using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LobbyAppearanceImprovements.CharacterSceneLayouts
{
    public class Moon2_DestroyedClone_PaladinOnly : CharSceneLayout
    {
        public override string SceneLayout => "Moon2_DestroyedClone_PaladinOnly";
        public override string SceneName => "Moon2";
        public override string Author => "DestroyedClone";
        public override string LayoutName => "Paladin's Entrance";
        public override Dictionary<string, Vector3[]> CharacterLayouts => new Dictionary<string, Vector3[]>()
        {
            { "Commando", new [] {new Vector3(2.65f, 0.01f, 6.00f), new Vector3(0f, 240f, 0f) } },
            { "Huntress", new [] {new Vector3(2.33f, 0.01f, 7.4f), new Vector3(0f, 240f, 0f) } },
            { "Bandit2", new [] {new Vector3(3.79f, 0.01f, 11.5f), new Vector3(0f, 240f, 0f) } },
            { "Engi", new [] {new Vector3(0.2f, 0f, 20.4f), new Vector3(0f, 240f, 0f) } },
            { "Merc", new [] {new Vector3(4.16f, 1.3f, 17f), new Vector3(0f, 240f, 0f) } },
            { "Loader", new [] {new Vector3(-4f, 0f, 20.22f), new Vector3(0f, 140f, 0f) } },
            { "Croco", new [] {new Vector3(-4f, 0f, 13f), new Vector3(0f, 120f, 0f) } },
        };
    }
}
