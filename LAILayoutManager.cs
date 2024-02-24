using LobbyAppearanceImprovements.Layouts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LobbyAppearanceImprovements
{
    internal static class LAILayoutManager
    {
        public static LAILayout chosenLayout = null;
        public static Dictionary<string, Type> layoutsDict = new Dictionary<string, Type>();
        public static List<string> layoutNameList = new List<string>();
        public static GameObject layoutInstance;
    }
}