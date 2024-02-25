using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    internal class SpaceCabin : LAIScene
    {
        public override string SceneNameToken => "LAI_MAP_LOBBYMULTI";

        public override GameObject BackgroundPrefab => SceneSetup.SpaceCabin;

        public override Vector3 Position => new Vector3(-4.5627f, 600f, - 432.1f);

        public override Quaternion Rotation => Quaternion.identity;

        public override Vector3 Scale => Vector3.one;

        //MainMenu/MENU: Multiplayer/World Position/HOLDER: Background/
        public override Material SkyboxOverride => LoadAsset<Material>("");
    }
}
