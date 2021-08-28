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
        };
    }
}
