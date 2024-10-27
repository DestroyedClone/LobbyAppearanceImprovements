using BepInEx;
using LobbyAppearanceImprovements.Layouts;
using LobbyAppearanceImprovements.MannequinLayouts;
using LobbyAppearanceImprovements.Scenes;
using R2API.Utils;
using RoR2;
using RoR2.UI;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using static LobbyAppearanceImprovements.ConfigSetup;
using static LobbyAppearanceImprovements.HookMethods;

namespace LobbyAppearanceImprovements
{
    internal class LAIPatches
    {
        public static void Init()
        {
            On.RoR2.UI.CharacterSelectController.OnEnable += CharacterSelectController_OnEnable;
            On.RoR2.UI.CharacterSelectController.OnDisable += CharacterSelectController_OnDisable;
        }
        private static void CharacterSelectController_OnEnable(On.RoR2.UI.CharacterSelectController.orig_OnEnable orig, CharacterSelectController self)
        {
            orig(self);
            On.RoR2.ShakeEmitter.ComputeTotalShakeAtPoint += ShakeEmitter_ComputeTotalShakeAtPoint;
        }

        private static Vector3 ShakeEmitter_ComputeTotalShakeAtPoint(On.RoR2.ShakeEmitter.orig_ComputeTotalShakeAtPoint orig, Vector3 position)
        {
            return orig(position) * (LAIPlugin.CharacterSelectController ? LAIPlugin.CharacterSelectController.localUser.userProfile.screenShakeScale : 1);
        }

        private static void CharacterSelectController_OnDisable(On.RoR2.UI.CharacterSelectController.orig_OnDisable orig, CharacterSelectController self)
        {
            On.RoR2.ShakeEmitter.ComputeTotalShakeAtPoint -= ShakeEmitter_ComputeTotalShakeAtPoint;
            orig(self);
        }
    }
}
