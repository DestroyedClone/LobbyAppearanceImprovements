using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;

namespace LobbyAppearanceImprovements.CharacterSceneLayouts
{
    public class Arena_DestroyedClone_Nemesis : CharSceneLayout
    {
        public override string SceneLayout => "Arena_DestroyedClone_Nemesis";
        public override string SceneName => "Arena";
        public override string Author => "DestroyedClone";
        public override string LayoutName => "Shadow of the Nemesis";
        public override Dictionary<string, Vector3[]> CharacterLayouts => new Dictionary<string, Vector3[]>()
        {
            { "Commando", new [] {new Vector3(2.65f, 0.01f, 6.00f), new Vector3(0f, 240f, 0f) } },
            { "Nemmando", new [] {new Vector3(3.37f, 0.1f, 5.68f), new Vector3(0f, 214f, 0f) } },
            //
            { "Enforcer", new [] {new Vector3(3.2f, 0f, 18.74f), new Vector3(0f, 220f, 0f) } },
            { "NemesisEnforcer", new [] {new Vector3(3f, 2.28f, 21f), new Vector3(0f, 200f, 0f) } },

            { "RobHenry", new [] {new Vector3(-4.5f, 1.22f, 8.81f), new Vector3(0f, 128f, 0f) } },
            { "RobHenry", new [] {new Vector3(-4.5f, 1.22f, 8.81f), new Vector3(0f, 128f, 0f) } },

        };
    }
}
