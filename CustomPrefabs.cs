using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;

namespace LobbyAppearanceImprovements
{
    public static class CustomPrefabs
    {
        public static Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

        public static GameObject GetCustomPrefab(string name)
        {
            return prefabs.TryGetValue(name, out GameObject value) ? value : null;
        }
    }
}
