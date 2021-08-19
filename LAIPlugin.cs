using BepInEx;
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
        public static ConfigEntry<string> SelectedScene { get; set; }

        // Survivors In Lobby //
        // Anything related to the config setting to show displays in the lobby //
        public static ConfigEntry<bool> SurvivorsInLobby { get; set; }
        public static ConfigEntry<int> SelectViewMode { get; set; }
        public static ConfigEntry<bool> ReplayAnim { get; set; }
        public static ConfigEntry<bool> LivePreview { get; set; }
        #endregion

        public void Awake()
        {
            SetupConfig();
        }

        public void SetupConfig()
        {
            // CONFIG //
            // Ordered by Layer //
            // UI //
            HideFade = Config.Bind("UI", "Hide Fade", true, "There's a dark fade on the top and bottom, this disables it.");
            BlurValue = Config.Bind("UI", "Adjust Blur (Not Implemented)", 255, "Adjusts the blur behind the UI elements on the left and right." +
                "\n0:fully transparent - 255:default");
            UIScale = Config.Bind("UI", "UI Scale", 1f, "Resizes the UIs on the left and right."); //def 1f

            // Overlay //
            // Anything that affects the scene as a whole //
            // Post Processing //
            PostProcessing = Config.Bind("UI", "Disable Post Processing", true, "Disables the blurry post processing.");

            // Lights //
            // The primary light over the scene //
            Light_Color = Config.Bind("Lights", "Hex Color", "default", "Change the default color of the light"); //#fa5a5a
            Light_Flicker_Disable = Config.Bind("Lights", "Disable FlickerLight", true, "Makes the light not flicker anymore.");
            Light_Intensity = Config.Bind("Lights", "Intensity", 1f, "Change the intensity of the light.");

            // Character Pad Displays //
            CharacterPadScale = Config.Bind("Background", "Character Display Scale", 1f, "Resizes character displays. "); //def 1f

            // Background Elements //
            // Anything in the background unrelated to the characters //
            MeshProps = Config.Bind("Background", "Hide MeshProps", false, "Hides all the background meshprops.");
            PhysicsProps = Config.Bind("Background", "Hide Physics Props", false, "Hides only the physics props like the Chair.");
            DisableShaking = Config.Bind("Background", "Disable Shaking", false, "Disables the random shaking that rattles the ship.");

            // Custom Background //
            SelectedScene = Config.Bind("Background", "Select Scene", "default", "Sets the current scene of the lobby.");

            // Survivors In Lobby //
            // Anything related to the config setting to show displays in the lobby //
            SurvivorsInLobby = Config.Bind("Background", "Survivors In Lobby", true, "Shows survivors in the lobby." +
                "\nThese background survivors don't reflect the loadouts in the lobby.");
            SelectViewMode = Config.Bind("Other", "Select View Mode (Requires SurvivorsInLobby set to true)", 0, "0 = None" +
                "\n1 = Disappear on selection" +
                "\n2 = Zoom on selection"); //def 1f
            ReplayAnim = Config.Bind("Background", "Replay Animation", true, "Replays the animation for the selected character.");
            LivePreview = Config.Bind("Background", "Live Preview", true, "Updates the appearance for the selected character.");
        }


    }
}
