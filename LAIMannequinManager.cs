using LobbyAppearanceImprovements.MannequinLayouts;
using RoR2.SurvivorMannequins;
using System;
using System.Collections.Generic;

namespace LobbyAppearanceImprovements
{
    internal static class LAIMannequinManager
    {
        public static SurvivorMannequinDioramaController mannequinDioramaController = null;

        public static BaseMannequinLayout chosenMannequinLayout = null;
        public static Dictionary<string, Type> mannequinLayoutsDict = new Dictionary<string, Type>();
        public static List<string> mannequinLayoutNameList = new List<string>();

        public static void Init()
        {
            On.RoR2.SurvivorMannequins.SurvivorMannequinDioramaController.OnEnable += (orig, self) =>
            {
                LAIMannequinManager.mannequinDioramaController = self;
                orig(self);
            };
        }
    }
}