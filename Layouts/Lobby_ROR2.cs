using System.Collections.Generic;
using UnityEngine;

namespace LobbyAppearanceImprovements.Layouts
{
    public class Lobby_ROR2 : LAILayout
    {
        public override string SceneLayout => "Lobby_ROR2";
        public override string SceneName => "Lobby";
        public override string Author => "DestroyedClone";
        public override string LayoutNameToken => "LAI_LAYOUT_ROR2";
        public override string ReadmeDescription => "Only the characters from RoR2";

        public override Dictionary<string, Vector3[]> CharacterLayouts => new Dictionary<string, Vector3[]>()
        {
            { "Commando", new [] {new Vector3(2.65f, 0.01f, 6.00f), new Vector3(0f, 240f, 0f) } },
            { "Bandit2", new [] {new Vector3(3.79f, 0.01f, 11.5f), new Vector3(0f, 240f, 0f) } },
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
            {"CommandoBody", new CameraSetting( 60, new Vector3(2.5f, 1.24f, 3) ) },
            {"HuntressBody", new CameraSetting( 60, new Vector3(3, 2.24f, 13.5f), new Vector3(0, 35, 0) ) },
            {"ToolbotBody", new CameraSetting( 60, new Vector3(-0.5f, 1.74f, 16.5f) ) },
            {"EngiBody", new CameraSetting( 60, new Vector3(-2.5f, 1.24f, 15) ) },
            {"MageBody", new CameraSetting( 60, new Vector3(3, 1.24f, 12) ) },
            {"MercBody", new CameraSetting( 60, new Vector3(-1.5f, 4.74f, 18.5f) ) },
            {"TreebotBody", new CameraSetting( 60, new Vector3(-5f, 0.24f, 20.5f), new Vector3(359.5351f, 322.6054f, 0) ) },
            {"LoaderBody", new CameraSetting( 60, new Vector3(4.5f, 1.24f, 10.5f) ) },
            {"CrocoBody", new CameraSetting( 60, new Vector3(5, 4.74f, 19) ) },
            {"CaptainBody", new CameraSetting( 60, new Vector3(2, 1.24f, 15) ) },
            {"RailgunnerBody", new CameraSetting( 60, new Vector3(1, 4.74f, 19) ) },
            {"VoidSurvivorBody", new CameraSetting( 60, new Vector3(3, 0.74f, 2.5f) ) },
            {"Bandit2Body", new CameraSetting( 60, new Vector3(3.5f, 1.74f, 9.5f) ) },
        };
    }
}