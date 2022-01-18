using System.Collections.Generic;
using UnityEngine;

namespace LobbyAppearanceImprovements.CharacterSceneLayouts
{
    public class BlackBeach_League : CharSceneLayout
    {
        public override string SceneLayout => "BlackBeach_League";
        public override string SceneName => "BlackBeach";
        public override string Author => "DestroyedClone";
        public override string LayoutName => "Risk of Legends";

        public override Dictionary<string, Vector3[]> CharacterLayouts => new Dictionary<string, Vector3[]>()
        {
            { "Twitch", new [] {new Vector3(-1.191686f, -0.4794466f, 4.918895f), new Vector3(0, 140f, 0f) } },
            { "Sett", new [] {new Vector3(-2.129966f, -0.4604257f, 10.40815f), new Vector3(0, 90f, 0f) } },
            { "Rengar", new [] {new Vector3(-1.116889f, -0.4532318f, 12.8763f), new Vector3(0, 140f, 0f) } },
            { "Pyke", new [] {new Vector3(0.9242006f, -0.4647151f, 8.61515f), new Vector3(0, 180f, 0f) } },
        };
        public override Dictionary<string, CameraSetting> CharacterCameraSettings => new Dictionary<string, CameraSetting>()
        {
            {"TwitchBody", new CameraSetting( 30, 8, -16 ) },
            {"SettBody", new CameraSetting( 20, -1, -11 ) },
            {"RengarBody", new CameraSetting( 20, 1, -5.5f ) },
            {"PykeBody", new CameraSetting( 20, 6, 5 ) },
        };
    }
}