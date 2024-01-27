using System.Collections.Generic;
using UnityEngine;

namespace LobbyAppearanceImprovements.CharacterSceneLayouts
{
    public class Any_Empty : CharSceneLayout
    {
        public override string SceneLayout => "Any_Empty";
        public override string SceneName => "Any";
        public override string Author => "DestroyedClone";
        public override string LayoutName => "";
        public override string ReadmeDescription => "The designated blank layout, though just toggle the layouts instead?";

        public override Dictionary<string, Vector3[]> CharacterLayouts => new Dictionary<string, Vector3[]>()
        {
        };
    }
}