﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LobbyAppearanceImprovements.CharacterSceneLayouts
{
    internal class CaptainsHelm_Default : CharSceneLayout
    {
        public override string SceneLayout => "CaptainsHelm_Default";
        public override string SceneName => "CaptainsHelm";
        public override string Author => "DestroyedClone";
        public override string LayoutNameToken => "LAI_LAYOUT_CAPTAINSHELM";
        public override string ReadmeDescription => "Default layout for Captain's Helm";

        public override Dictionary<string, Vector3[]> CharacterLayouts => new Dictionary<string, Vector3[]>();
        public override Dictionary<string, CameraSetting> CharacterCameraSettings => new Dictionary<string, CameraSetting>()
        {
            { "CaptainBody", new CameraSetting(60f, new Vector3(0.05f, 1.79f, 2.45f)) }
        };
    }
}
