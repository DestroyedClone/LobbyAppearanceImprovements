using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LobbyAppearanceImprovements.MannequinLayouts
{
    public abstract class BaseMannequinLayout
    {
        public BaseMannequinLayout()
        {

        }

        // Technical Name of the scene
        public abstract string MannequinLayout { get; }

        // Name of the Scene : "Lobby"
        public abstract string SceneName { get; }

        // Author of the scene : "DestroyedClone"
        public abstract string Author { get; }

        // Name of the layout : "Original Crew"
        public abstract string LayoutName { get; }

        public abstract List<TransformInfo> MannequinTransforms { get; }

        public struct TransformInfo
        {
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 scale;
        }

        public void CreateMannequinLayout()
        {

        }
    }
}
