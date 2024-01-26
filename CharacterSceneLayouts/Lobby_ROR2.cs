using System.Collections.Generic;
using UnityEngine;

namespace LobbyAppearanceImprovements.CharacterSceneLayouts
{
    public class Lobby_ROR2 : CharSceneLayout
    {
        public override string SceneLayout => "Lobby_ROR2";
        public override string SceneName => "Lobby";
        public override string Author => "DestroyedClone";
        public override string LayoutName => "ROR2";

        public override Dictionary<string, Vector3[]> CharacterLayouts => new Dictionary<string, Vector3[]>()
        {
            { "Commando", new [] {new Vector3(2.65f, 0.01f, 6.00f), new Vector3(0f, 240f, 0f) } },
            { "Huntress", new [] {new Vector3(4.8f, 1.43f, 15.36f), new Vector3(0f, 170f, 0f) } },
            { "Toolbot", new [] {new Vector3(-0.21f, 0.15f, 20.84f), new Vector3(0f, 170f, 0f) } },
            { "Engi", new [] {new Vector3(-2.58f, -0.01f, 19f), new Vector3(0f, 150f, 0f) } },
            { "Mage", new [] {new Vector3(3.35f, 0.21f, 14.73f), new Vector3(0f, 220f, 0f) } },
            { "Merc", new [] {new Vector3(-1.32f, 3.65f, 22.28f), new Vector3(0f, 180f, 0f) } },
            { "Treebot", new [] {new Vector3(-6.51f, -0.11f, 22.93f), new Vector3(0f, 140f, 0f) } },
            { "Loader", new [] {new Vector3(5.04f, 0, 14.26f), new Vector3(0f, 220f, 0f) } },
            { "Croco", new [] {new Vector3(5f, 3.59f, 22f), new Vector3(0f, 210f, 0f) } },
            { "Captain", new [] {new Vector3(2.21f, 0.01f, 19.40f), new Vector3(0f, 190f, 0f) } },
            { "Railgunner", new [] {new Vector3(1.3f, 3.6595f, 22.5774f), new Vector3(0f, 203f, 0f) } },
            { "VoidSurvivor", new [] {new Vector3(3f, 0f, 5.5f), new Vector3(0f, 290f, 0f) } },
        };

        public override Dictionary<string, CameraSetting> CharacterCameraSettings => new Dictionary<string, CameraSetting>()
        {
            {"CommandoBody", new CameraSetting( 20, 2, 24 ) },
            {"HuntressBody", new CameraSetting( 9, -3, 18 ) },
            {"ToolbotBody", new CameraSetting( 10, -1, -1 ) },
            {"EngiBody", new CameraSetting( 6, 1, -7.5f ) },
            {"MageBody", new CameraSetting( 8, -1, 13 ) },
            {"MercBody", new CameraSetting( 5, -8.5f, -3 ) },
            {"TreebotBody", new CameraSetting( 6, 0.7f, -15.5f ) },
            {"LoaderBody", new CameraSetting( 11, -2, 20 ) },
            {"CrocoBody", new CameraSetting( 8, -8.5f, 13 ) },
            {"CaptainBody", new CameraSetting( 9, -1, 6 ) },
            {"Railgunner", new CameraSetting( 9, -1, 6 ) },
            {"VoidSurvivor", new CameraSetting( 9, -1, 6 ) },
        };
    }
}