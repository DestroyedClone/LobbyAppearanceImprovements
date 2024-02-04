using LobbyAppearanceImprovements.CharacterSceneLayouts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LobbyAppearanceImprovements
{
    internal static class LAILayoutManager
    {
        public static CharSceneLayout chosenLayout = null;
        public static Dictionary<string, Type> layoutsDict = new Dictionary<string, Type>();
        public static List<string> layoutNameList = new List<string>();
        public static GameObject layoutInstance;
    }
}