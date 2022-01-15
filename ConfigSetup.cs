using BepInEx.Configuration;
using InLobbyConfig;
using InLobbyConfig.Fields;
using System.Collections;
using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using static LobbyAppearanceImprovements.HookMethods;

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
        public static ConfigEntry<bool> Parallax { get; set; }
        public static ConfigEntry<bool> EnableCharacterRotation { get; set; }

        // Lights //
        // The primary light over the scene //
        public static ConfigEntry<string> Light_Color { get; set; }

        public static ConfigEntry<bool> Light_Flicker { get; set; }
        public static ConfigEntry<float> Light_Intensity { get; set; }

        // Character Pad Displays //
        public static ConfigEntry<float> CharacterPadScale { get; set; }

        // Background Elements //
        // Anything in the background unrelated to the characters //
        public static ConfigEntry<bool> MeshProps { get; set; }

        public static ConfigEntry<bool> PhysicsProps { get; set; }
        public static ConfigEntry<bool> Shaking { get; set; }

        // Custom Background //
        public static ConfigEntry<string> SelectedScene { get; set; }

        // Survivors In Lobby //
        // Anything related to the config setting to show displays in the lobby //
        public static ConfigEntry<bool> SIL_Enabled { get; set; }
        public static ConfigEntry<bool> SIL_LockedCharactersBlack { get; set; }
        public static ConfigEntry<string> SIL_SelectedLayout { get; set; }
        public static ConfigEntry<bool> SIL_ZoomEnable { get; set; }
        public static ConfigEntry<bool> SIL_ClickOnCharacterToSwap { get; set; }
        //public static ConfigEntry<bool> ReplayAnim { get; set; }
        //public static ConfigEntry<bool> LivePreview { get; set; }

        // InLobbyConfig //

        private static ModConfigEntry inLobbyConfigEntry;

        // Debugging //
        public static ConfigEntry<LoggingStyle> ShowLoggingText { get; set; }

        #endregion Config

        #region tempvalues
        public static string tempSceneName;
        public static string tempLayoutName;
        public static bool tempConfirmChoice;
        #endregion


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
            PostProcessing = config.Bind("Overlay", "Post Processing", true, "Toggles the blurry post processing.");
            Parallax = config.Bind("Overlay", "Parallax", true, "Enable to toggle on a slight parallax effect controlled by the position of the cursor.");
            EnableCharacterRotation = config.Bind("Overlay", "Character Rotation", true, "Clicking and dragging will rotate the frontmost character.");

            // Lights //
            // The primary light over the scene //
            Light_Color = config.Bind("Lights", "Hex Color", "default", "Change the default color of the light, include the # for hex values"); //#fa5a5a
            Light_Flicker = config.Bind("Lights", "Flickerlight", true, "Makes the light not flicker anymore.");
            Light_Intensity = config.Bind("Lights", "Intensity", 1f, "Change the intensity of the light.");

            // Character Pad Displays //
            CharacterPadScale = config.Bind("Background", "Character Display Scale", 1f, "Resizes character displays. "); //def 1f

            // Background Elements //
            // Anything in the background unrelated to the characters //
            MeshProps = config.Bind("Background", "Show Static MeshProps", false, "Toggles all the stationary meshprops.");
            PhysicsProps = config.Bind("Background", "Show Physics Props", false, "Toggles all the physics props like the Chair.");
            Shaking = config.Bind("Background", "Shaking", false, "Toggles the random shaking that rattles the ship.");

            // Custom Background //
            SelectedScene = config.Bind("Background", "Select Scene", "default", "Sets the current scene of the lobby.");

            // Survivors In Lobby //
            // Anything related to the config setting to show displays in the lobby //
            SIL_Enabled = config.Bind("Background", "Enable Character Layouts", true, "Shows background elements in certain orientations." +
                "\nThese background survivors don't reflect the loadouts in the lobby.");
            SIL_LockedCharactersBlack = config.Bind("Background", "Enable Unavailable Shadow Survivors", true, "If true, any survivors in a character layout that you don't have unlocked become shadowy.");
            SIL_SelectedLayout = config.Bind("Background", "Character Layout Name", "default", "Name of the layout to set to.");
            SIL_ZoomEnable = config.Bind("Background", "Zoom On Character Select", true, "If true, selecting a character will zoom the camera onto that character.");
            /*SelectViewMode = config.Bind("Background", "X Select View Mode (Requires SurvivorsInLobby set to true)", 0, "0 = None" +
                "\n1 = Disappear on selection" +
                "\n2 = Zoom on selection"); //def 1f*/
            //ReplayAnim = config.Bind("Background", "X Replay Animation", true, "Replays the animation for the selected character.");
            //LivePreview = config.Bind("Background", "X Live Preview", true, "Updates the appearance for the selected character.");
            SIL_ClickOnCharacterToSwap = config.Bind("Background", "Click on bg char to select (EXPERIMENTAL)", true, "Allows clicking on a character to select them." +
                "\nExperimental: Clicking on the character might be unavailable, or offset.");

            ShowLoggingText = config.Bind("zDebugging", "Print logging text to console", LoggingStyle.Minimal, "If true, then some logging messages are sent to the console.");

            tempSelectSceneAction += SetNewScene;
            tempSelectLayoutAction += SetNewLayout;

            tempSceneName = SelectedScene.Value;
            tempLayoutName = SIL_SelectedLayout.Value;
        }

        public enum LoggingStyle
        {
            None,
            Minimal,
            All
        }

        public static void InLobbyBind()
        {
            inLobbyConfigEntry = new ModConfigEntry
            {
                DisplayName = "Lobby Appearance Improvements",
            };
            inLobbyConfigEntry.SectionFields["UI"] = new List<IConfigField>
            {
                new BooleanConfigField(UI_ShowFade.Definition.Key, UI_ShowFade.Description.Description, () => UI_ShowFade.Value, Hook_ShowFade),
                new IntConfigField(UI_BlurOpacity.Definition.Key, () => UI_BlurOpacity.Value, Hook_BlurOpacity, null, 0, 255),
                new FloatConfigField(UI_Scale.Definition.Key, () => UI_Scale.Value, null, Hook_UIScale, 0.5f)
            };
            inLobbyConfigEntry.SectionFields["Overlay"] = new List<IConfigField>
            {
                new BooleanConfigField(PostProcessing.Definition.Key, PostProcessing.Description.Description, () => PostProcessing.Value, Hook_ShowPostProcessing),
                new BooleanConfigField(Parallax.Definition.Key, Parallax.Description.Description, () => Parallax.Value, Hook_Parallax),
            };
            inLobbyConfigEntry.SectionFields["Lights"] = new List<IConfigField>
            {
                new StringConfigField(Light_Color.Definition.Key, Light_Color.Description.Description, () => Light_Color.Value, Hook_LightUpdate_Color),
                new BooleanConfigField(Light_Flicker.Definition.Key, Light_Flicker.Description.Description, () => Light_Flicker.Value, Hook_LightUpdate_Flicker),
                new FloatConfigField(Light_Intensity.Definition.Key, Light_Intensity.Description.Description, () => Light_Intensity.Value, Hook_LightUpdate_Intensity)
            };
            inLobbyConfigEntry.SectionFields["Character Pads"] = new List<IConfigField>
            {
                new FloatConfigField(CharacterPadScale.Definition.Key, CharacterPadScale.Description.Description, () => CharacterPadScale.Value, Hook_RescalePads)
            };
            inLobbyConfigEntry.SectionFields["Background Elements"] = new List<IConfigField>
            {
                new BooleanConfigField(MeshProps.Definition.Key, MeshProps.Description.Description, () => MeshProps.Value, Hook_HideProps),
                new BooleanConfigField(PhysicsProps.Definition.Key, PhysicsProps.Description.Description, () => PhysicsProps.Value, Hook_HidePhysicsProps),
                new BooleanConfigField(Shaking.Definition.Key, Shaking.Description.Description, () => Shaking.Value, Hook_DisableShaking),
            };
            inLobbyConfigEntry.SectionFields["Scenes+Layouts"] = new List<IConfigField>
            {
                //new SelectListField<string>(SelectedScene.Definition.Key, SelectedScene.Description.Description, SceneMethods.GetScenes, null, null, null),
                new StringConfigField(SelectedScene.Definition.Key, SelectedScene.Description.Description, () => SelectedScene.Value, null, tempSelectSceneAction),
                new StringConfigField(SIL_SelectedLayout.Definition.Key, SIL_SelectedLayout.Description.Description, () => SIL_SelectedLayout.Value, null, tempSelectLayoutAction),
                new BooleanConfigField("Confirm Choice", "Click to confirm choice for scene.", () => tempConfirmChoice, SetSceneLayoutFromLobby),
                new BooleanConfigField(SIL_Enabled.Definition.Key, SIL_Enabled.Description.Description, () => SIL_Enabled.Value, Hook_SurvivorsInLobby),
                new BooleanConfigField(SIL_ZoomEnable.Definition.Key, SIL_ZoomEnable.Description.Description, () => SIL_ZoomEnable.Value, Hook_ToggleZooming),
                new BooleanConfigField(SIL_LockedCharactersBlack.Definition.Key, SIL_LockedCharactersBlack.Description.Description, () => SIL_LockedCharactersBlack.Value, Hook_BlackenSurvivors),
            };
            ModConfigCatalog.Add(inLobbyConfigEntry);
        }

        public static Action<string> tempSelectSceneAction;
        public static Action<string> tempSelectLayoutAction;
        public static void SetNewScene(string value)
        {
            tempSceneName = value;
        }
        public static void SetNewLayout(string value)
        {
            tempLayoutName = value;
        }
        public static void SetSceneLayoutFromLobby(bool value)
        {
            if (value)
            {
                var result = Methods.LoadSceneAndLayout(tempSceneName, tempLayoutName);
                tempConfirmChoice = false;

                switch (result)
                {
                    case Methods.LoadSceneAndLayoutResult.NoSceneNoLayout:
                        break;
                    case Methods.LoadSceneAndLayoutResult.NoScene:
                        SIL_SelectedLayout.Value = tempLayoutName;
                        break;
                    case Methods.LoadSceneAndLayoutResult.NoLayout:
                        SelectedScene.Value = tempSceneName;
                        break;
                    default:
                        SelectedScene.Value = tempSceneName;
                        SIL_SelectedLayout.Value = tempLayoutName;
                        break;
                }
            }
        }
    }
}