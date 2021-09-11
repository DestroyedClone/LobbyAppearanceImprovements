using BepInEx.Configuration;
using InLobbyConfig;
using InLobbyConfig.Fields;
using System.Collections;
using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace LobbyAppearanceImprovements
{
    public static class ConfigSetup
    {
        #region Config

        // CONFIG //
        // Ordered by Layer //
        // UI //
        public static ConfigEntry<bool> UI_ShowFade { get; set; }
        public static ConfigEntry<int> UI_BlurOpacity { get; set; }
        public static ConfigEntry<float> UI_Scale { get; set; }

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
        public static ConfigEntry<bool> SIL_Enabled { get; set; }
        public static ConfigEntry<bool> SIL_LockedCharactersBlack { get; set; }
        public static ConfigEntry<string> SIL_SelectedLayout { get; set; }
        public static ConfigEntry<int> SelectViewMode { get; set; }
        public static ConfigEntry<bool> SIL_ClickOnCharacterToSwap { get; set; }
        public static ConfigEntry<bool> ReplayAnim { get; set; }
        public static ConfigEntry<bool> LivePreview { get; set; }

        // InLobbyConfig //

        private static ModConfigEntry inLobbyConfigEntry;

        #endregion Config



        public static void Bind(ConfigFile config)
        {
            // CONFIG //
            // Ordered by Layer //
            // UI //
            UI_ShowFade = config.Bind("UI", "Show Fade", true, "Toggles the dark fade bars at the top and bottom of the lobby.");
            UI_BlurOpacity = config.Bind("UI", "Blur Opacity", 255, "Adjusts the blur opacity behind the UI elements on the left and right." +
                "\n0:fully transparent - 255:default");
            UI_Scale = config.Bind("UI", "UI Scale", 1f, "Resizes the UIs on the left and right."); //def 1f

            // Overlay //
            // Anything that affects the scene as a whole //
            // Post Processing //
            PostProcessing = config.Bind("UI", "Disable Post Processing", true, "Disables the blurry post processing.");

            // Lights //
            // The primary light over the scene //
            Light_Color = config.Bind("Lights", "Hex Color", "default", "Change the default color of the light, include the # for hex values"); //#fa5a5a
            Light_Flicker_Disable = config.Bind("Lights", "Disable FlickerLight", true, "Makes the light not flicker anymore.");
            Light_Intensity = config.Bind("Lights", "Intensity", 1f, "Change the intensity of the light.");

            // Character Pad Displays //
            CharacterPadScale = config.Bind("Background", "Character Display Scale", 1f, "Resizes character displays. "); //def 1f

            // Background Elements //
            // Anything in the background unrelated to the characters //
            MeshProps = config.Bind("Background", "Hide Static MeshProps", false, "Hides all the stationary meshprops.");
            PhysicsProps = config.Bind("Background", "Hide Physics Props", false, "Hides all the physics props like the Chair.");
            DisableShaking = config.Bind("Background", "Disable Shaking", false, "Disables the random shaking that rattles the ship.");

            // Custom Background //
            SelectedScene = config.Bind("Background", "Select Scene", "default", "Sets the current scene of the lobby.");

            // Survivors In Lobby //
            // Anything related to the config setting to show displays in the lobby //
            SIL_Enabled = config.Bind("Background", "Survivors In Lobby", true, "Shows survivors in the lobby." +
                "\nThese background survivors don't reflect the loadouts in the lobby.");
            SIL_LockedCharactersBlack = config.Bind("Background", "Lobby Survivors", true, "If survivors are in the lobby, then blacks out the ones you don't have unlocked..");
            SIL_SelectedLayout = config.Bind("Background", "Survivors In Lobby Layout", "default", "Layout of the survivors in the scene.");
            SelectViewMode = config.Bind("Background", "X Select View Mode (Requires SurvivorsInLobby set to true)", 0, "0 = None" +
                "\n1 = Disappear on selection" +
                "\n2 = Zoom on selection"); //def 1f
            ReplayAnim = config.Bind("Background", "X Replay Animation", true, "Replays the animation for the selected character.");
            LivePreview = config.Bind("Background", "X Live Preview", true, "Updates the appearance for the selected character.");
            SIL_ClickOnCharacterToSwap = config.Bind("Background", "Click on bg char to select (EXPERIMENTAL)", true, "Allows clicking on a character to select them.");
        }

        public static void InLobbyBind()
        {
            inLobbyConfigEntry = new ModConfigEntry
            {
                DisplayName = "Lobby Appearance Improvements",
            };
            inLobbyConfigEntry.SectionFields["UI"] = new List<IConfigField>
            {
                new BooleanConfigField(UI_ShowFade.Definition.Key, UI_ShowFade.Description.Description, () => UI_ShowFade.Value, LAIPlugin.Hook_ShowFade),
                new IntConfigField(UI_BlurOpacity.Definition.Key, () => UI_BlurOpacity.Value, LAIPlugin.Hook_BlurOpacity, null, 0, 255),
                new FloatConfigField(UI_Scale.Definition.Key, () => UI_Scale.Value, null, LAIPlugin.Hook_UIScale, 0.1f)
            };
            ModConfigCatalog.Add(inLobbyConfigEntry);
        }
    }
}