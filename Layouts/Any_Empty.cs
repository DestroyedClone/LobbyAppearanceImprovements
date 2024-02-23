﻿using System.Collections.Generic;
using UnityEngine;

namespace LobbyAppearanceImprovements.Layouts
{
    public class Any_Empty : LAILayout
    {
        public override string SceneLayout => "Any_Empty";
        public override string SceneName => "Any";
        public override string Author => "DestroyedClone";
        public override string ReadmeDescription => "The designated blank layout.";

        public override string LayoutNameToken => "LAI_EMPTY";

        public override Dictionary<string, Vector3[]> CharacterLayouts => new Dictionary<string, Vector3[]>()
        {
        };
    }
}