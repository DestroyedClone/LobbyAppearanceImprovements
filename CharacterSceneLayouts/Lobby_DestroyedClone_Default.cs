using UnityEngine;
using System.Collections.Generic;

namespace LobbyAppearanceImprovements.CharacterSceneLayouts
{
    public class Lobby_DestroyedClone_Default : CharSceneLayout
    {
        public override string SceneLayout => "Lobby_DestroyedClone_Default";
        public override string SceneName => "Lobby";
        public override string Author => "DestroyedClone";
        public override string LayoutName => "Default";
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
            //
            { "Enforcer", new [] {new Vector3(3.2f, 0f, 18.74f), new Vector3(0f, 220f, 0f) } },
            { "NemesisEnforcer", new [] {new Vector3(3f, 2.28f, 21f), new Vector3(0f, 200f, 0f) } },

            { "SniperClassic", new [] { new Vector3(-5f, 0.03f, 22f), new Vector3(0f, 180f, 0f) } },
            { "BanditReloaded", new [] {new Vector3(-3.5f, -0.06f, 5.85f), new Vector3(0f, 154f, 0f) } },
            { "HANDOverclocked", new [] { new Vector3(-1.57f, -0.038f, 20.48f), new Vector3(0f, 154f, 0f) } },
            { "Miner", new [] {new Vector3(-3.3f, 0.04f, 6.69f), new Vector3(0f, 140f, 0f) } },
            { "RobPaladin", new [] {new Vector3(-4f, 0.01f, 22f), new Vector3(0f, 160f, 0f) } },
            { "CHEF", new [] {new Vector3(1.63f, 3.4f, 23.2f), new Vector3(0f, 270f, 0f) } },
            { "RobHenry", new [] {new Vector3(-4.5f, 1.22f, 8.81f), new Vector3(0f, 128f, 0f) } },
            { "Wyatt", new [] {new Vector3(-3.92f, 0.1f, 9.62f), new Vector3(0f, 138f, 0f) } },
            { "Custodian", new [] {new Vector3(-3.92f, 0.1f, 9.62f), new Vector3(0f, 138f, 0f) } },
            { "Executioner", new [] {new Vector3(1.19f, 0.01f, 19.74f), new Vector3(0f, 192f, 0f) } },
            { "Nemmando", new [] {new Vector3(3.37f, 0.1f, 5.68f), new Vector3(0f, 214f, 0f) } },
        };

        public override Dictionary<string, CameraSetting> CharacterCameraSettings => new Dictionary<string, CameraSetting>()
        {
            {"Commando", new CameraSetting( 20, 2, 24 ) },
            {"Huntress", new CameraSetting( 9, -3, 18 ) },
            {"Toolbot", new CameraSetting( 10, -1, -1 ) },
            {"Engi", new CameraSetting( 6, 1, -7.5f ) },
            {"Mage", new CameraSetting( 8, -1, 13 ) },
            {"Merc", new CameraSetting( 5, -8.5f, -3 ) },
            {"Treebot", new CameraSetting( 6, 0.7f, -15.5f ) },
            {"Loader", new CameraSetting( 11, -2, 20 ) },
            {"Croco", new CameraSetting( 8, -8.5f, 13 ) },
            {"Captain", new CameraSetting( 9, -1, 6 ) },
            {"SniperClassic", new CameraSetting( 6, 0.5f, -12.5f ) },
            {"Enforcer", new CameraSetting( 11, -1, 10 ) },
            {"NemesisEnforcer", new CameraSetting( 10, -7.5f, 8 ) },
            {"BanditReloaded", new CameraSetting( 20, 1, -30 ) },
            {"HANDOverclocked", new CameraSetting( 8, -2, -4 ) },
            {"Miner", new CameraSetting( 17, 1, -26 ) },
            {"RobPaladin", new CameraSetting( 9, -1, -10 ) },
            {"CHEF", new CameraSetting( 5, -8.5f, 3 ) },
            {"RobHenry", new CameraSetting( 12, -7, -27 ) },
            {"Wyatt", new CameraSetting( 12, -2, -22 ) },
            {"Custodian", new CameraSetting( 12, -2, -22 ) },
            {"Executioner", new CameraSetting( 8, -0.25f, 3.75f ) },
            {"Nemmando", new CameraSetting( 20, 1, 30 ) },
        };
    }
}