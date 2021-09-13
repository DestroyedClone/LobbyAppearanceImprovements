using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;

namespace LobbyAppearanceImprovements.CharacterSceneLayouts
{
    public class Any_Empty : CharSceneLayout
    {
        public override string SceneLayout => "Any_Empty";
        public override string SceneName => "Any";
        public override string Author => "DestroyedClone";
        public override string LayoutName => "";
        public override Dictionary<string, Vector3[]> CharacterLayouts => new Dictionary<string, Vector3[]>()
        {
        };
    }
}
