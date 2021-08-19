﻿using BepInEx;
using BepInEx.Configuration;
using LeTai.Asset.TranslucentImage;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using static UnityEngine.ColorUtility;
[module: UnverifiableCode]
#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete

namespace LobbyAppearanceImprovements
{
    [BepInDependency(R2API.R2API.PluginGUID, R2API.R2API.PluginVersion)]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    [BepInPlugin(ModGuid, ModName, ModVer)]
    public class LAIPlugin : BaseUnityPlugin
    {
        public const string ModVer = "1.1.0";
        public const string ModName = "LobbyAppearanceImprovements";
        public const string ModGuid = "com.DestroyedClone.LobbyAppearanceImprovements";

        #region Config
        // CONFIG //
        // Ordered by Layer //
        // UI //
        public static ConfigEntry<bool> HideFade { get; set; }
        public static ConfigEntry<int> BlurValue { get; set; }
        public static ConfigEntry<float> UIScale { get; set; }

        // Overlay //
        // Anything that affects the scene as a whole //
        // Post Processing //
        public static ConfigEntry<bool> PostProcessing { get; set; }

        // Lights //
        // The primary light over the scene //
        public static ConfigEntry<string> Light_Color { get; set; }
        public static ConfigEntry<bool> Light_Flicker_Disable { get; set; }
        public static ConfigEntry<float> Light_Intensity { get; set; }

        // Character Pad Displays //
        public static ConfigEntry<float> CharacterPadScale { get; set; }

        // Background Elements //
        // Anything in the background unrelated to the characters //
        public static ConfigEntry<bool> MeshProps { get; set; }
        public static ConfigEntry<bool> PhysicsProps { get; set; }
        public static ConfigEntry<bool> DisableShaking { get; set; }

        // Custom Background //
        public static ConfigEntry<int> SelectedScene { get; set; }

        // Survivors In Lobby //
        // Anything related to the config setting to show displays in the lobby //
        public static ConfigEntry<bool> SurvivorsInLobby { get; set; }
        public static ConfigEntry<int> SelectViewMode { get; set; }
        public static ConfigEntry<bool> ReplayAnim { get; set; }
        public static ConfigEntry<bool> LivePreview { get; set; }
        #endregion

        public void Awake()
        {

        }
    }
}
