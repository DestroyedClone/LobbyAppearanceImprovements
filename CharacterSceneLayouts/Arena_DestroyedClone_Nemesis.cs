﻿using System;
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
            { "Commando", new [] {new Vector3(0f, 0f, 7f), new Vector3(0f, 60f, 0f) } },
            { "Nemmando", new [] {new Vector3(-0.3f, 0f, 6.6f), new Vector3(0f, 214f, 0f) } },
            //
            { "Enforcer", new [] {new Vector3(2f, 0f, 12f), new Vector3(0f, 220f, 0f) } },
            { "NemesisEnforcer", new [] {new Vector3(4f, 0f, 12f), new Vector3(0f, 170f, 0f) } },

            { "RobHenry", new [] {new Vector3(-2f, 0f, 4f), new Vector3(0f, 180f, 0f) } },
            { "Nemry", new [] {new Vector3(-2f, 0f, 8f), new Vector3(0f, 128f, 0f) } },

        };
    }
}
