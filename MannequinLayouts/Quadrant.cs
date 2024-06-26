using System.Collections.Generic;
using UnityEngine;

namespace LobbyAppearanceImprovements.MannequinLayouts
{
    public class Quadrant : BaseMannequinLayout
    {
        public override string NameToken => "QUADRANT";

        public override string InternalName => "Quadrant";

        public override List<TransformInfo> MannequinTransforms => new List<TransformInfo>()
        {
            new TransformInfo(new Vector3(), new Vector3(), Vector3.one),
            new TransformInfo(new Vector3(), new Vector3(), Vector3.one),
            new TransformInfo(new Vector3(), new Vector3(), Vector3.one),
            new TransformInfo(new Vector3(), new Vector3(), Vector3.one),
        };
    }
}