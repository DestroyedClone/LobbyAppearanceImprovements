using BepInEx;
using LobbyAppearanceImprovements;
using LobbyAppearanceImprovements.CharacterSceneLayouts;
using LobbyAppearanceImprovements.Scenes;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.SurvivorMannequins;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using static LobbyAppearanceImprovements.ConfigSetup;
using static LobbyAppearanceImprovements.HookMethods;

namespace LobbyAppearanceImprovements
{
    internal static class LAISceneManager
    {
        public static LAIScene chosenScene = null;
        public static Dictionary<string, Type> scenesDict = new Dictionary<string, Type>();
        public static List<string> sceneNameList = new List<string>();
        public static GameObject sceneInstance;

        public static void Initialize()
        {
            SceneSetup.Init();
            On.RoR2.PreGameController.RefreshLobbyBackground += PreGameController_RefreshLobbyBackground;
        }

        private static void PreGameController_RefreshLobbyBackground(On.RoR2.PreGameController.orig_RefreshLobbyBackground orig, PreGameController self)
        {
            orig(self);
            if (self.lobbyBackground) self.lobbyBackground.SetActive(false);
        }
    }
}