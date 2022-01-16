using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LobbyAppearanceImprovements.CharacterSceneLayouts
{
    public class WispGraveyard_Snipers : CharSceneLayout
    {
        public override string SceneLayout => "WispGraveyard_Snipers";
        public override string SceneName => "WispGraveyard";
        public override string Author => "DestroyedClone";
        public override string LayoutName => "I found you, faker!";

        public override string[] RequiredModGUID => new string[]
        {
            "com.Moffein.SniperClassic",
            "Rein.Sniper"
        };

        public override Dictionary<string, Vector3[]> CharacterLayouts => new Dictionary<string, Vector3[]>()
        {
            { "!Sniper", new [] {new Vector3(-1.287264f, 6.053263f, 13.60413f), new Vector3(40, 170f, 0f) } },
            { "SniperClassic", new [] {new Vector3(-1.377686f, 0.06645632f, 5.194136f), new Vector3(350, 0f, 0f) } },
        };
    }
}
