using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LobbyAppearanceImprovements.CharacterSceneSetups
{
    public abstract class CharSceneLayout
    {
        public abstract string SceneLayout { get; }
        public abstract string SceneName { get; }
        public abstract string Author { get; }
        public abstract string LayoutName { get; }
    }
}
