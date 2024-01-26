using System.Collections.Generic;
using UnityEngine;

namespace LobbyAppearanceImprovements.CharacterSceneLayouts
{
    public class Lobby_Original : CharSceneLayout
    {
        public override string SceneLayout => "Lobby_Original";
        public override string SceneName => "Lobby";
        public override string Author => "DestroyedClone";
        public override string LayoutName => "The Original";

        public override Dictionary<string, Vector3[]> CharacterLayouts => new Dictionary<string, Vector3[]>()
        {
            { "Commando", new [] {new Vector3(2.65f, 0.01f, 6.00f), new Vector3(0f, 240f, 0f) } },
            { "Enforcer", new [] {new Vector3(3.2f, 0f, 18.74f), new Vector3(0f, 220f, 0f) } },
            { "BanditReloaded", new [] {new Vector3(2.2f, 0f, 19.4f), new Vector3(0f, 220f, 0f) } },
            { "Bandit2", new [] {new Vector3(3.79f, 0.01f, 11.5f), new Vector3(0f, 240f, 0f) } },
            { "Huntress", new [] {new Vector3(2.33f, 0.01f, 7.4f), new Vector3(0f, 240f, 0f) } },
            { "HANDOverclocked", new [] {new Vector3(-2.2f, 0f, 21f), new Vector3(0f, 180f, 0f) } },

            { "Engi", new [] {new Vector3(0.2f, 0f, 19f), new Vector3(0f, 180f, 0f) } },
            { "Miner", new [] {new Vector3(0f, 0f, 19f), new Vector3(0f, 180f, 0f) } },
            { "SniperClassic", new [] { new Vector3(-5f, 2.18f, 23f), new Vector3(0f, 180f, 0f) } },
            { "Croco", new [] {new Vector3(-4f, 0f, 13f), new Vector3(0f, 120f, 0f) } },
            { "Merc", new [] {new Vector3(4.16f, 1.3f, 17f), new Vector3(0f, 240f, 0f) } },
            { "Loader", new [] {new Vector3(-4f, 0f, 20.22f), new Vector3(0f, 140f, 0f) } },
            { "!CHEF", new [] {new Vector3(1.63f, 3.4f, 23.2f), new Vector3(0f, 270f, 0f) } },
        };

        public override Dictionary<string, CameraSetting> CharacterCameraSettings => new Dictionary<string, CameraSetting>()
        {
            {"CommandoBody", new CameraSetting( 20, 0, 22 ) },
            {"EnforcerBody", new CameraSetting( 10, -1, 10 ) },
            {"BanditReloadedBody", new CameraSetting( 10, 0, 6 ) },
            {"Bandit2Body", new CameraSetting( 15, -1, 17.5f ) },
            {"HuntressBody", new CameraSetting( 20, 4, 18 ) },
            {"HANDOverclockedBody", new CameraSetting( 10, -1, -7 ) },

            {"EngiBody", new CameraSetting( 10, 2, 0 ) },
            {"MinerBody", new CameraSetting( 6, 0, 0 ) },
            {"SniperClassicBody", new CameraSetting( 5, -5, -12 ) },
            {"CrocoBody", new CameraSetting( 20, 0, -18 ) },
            {"MercBody", new CameraSetting( 10, -3, 13 ) },
            {"LoaderBody", new CameraSetting( 9, -1, -11 ) },
            {"CHEF", new CameraSetting( 7, -9, 2 ) },
        };
    }
}