using LobbyAppearanceImprovements.Scenes;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

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