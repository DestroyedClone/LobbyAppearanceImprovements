using BepInEx.Configuration;
using InLobbyConfig;
using InLobbyConfig.Fields;
using System;
using System.Collections.Generic;
using UnityEngine;
using static LobbyAppearanceImprovements.HookMethods;

namespace LobbyAppearanceImprovements
{
    public static class ConfigSetup
    {
        public static void Initialize(ConfigFile configFile)
        {
            Bind(configFile);
            InLobbyBind();
        }

        #region Config

        // CONFIG //
        // Ordered by Layer //
        // UI //
        public static ConfigEntry<bool> UI_ShowFade { get; set; }

        public static ConfigEntry<int> UI_BlurOpacity { get; set; }
        public static ConfigEntry<float> UI_Scale { get; set; }

        public static float UI_Scale_Min = 0.5f;
        public static float UI_Scale_Max = 1.75f;

        // Overlay //
        public static ConfigEntry<string> MusicChoice { get; set; }

        // Anything that affects the scene as a whole //
        // Post Processing //
        public static ConfigEntry<bool> PostProcessing { get; set; }

        public static ConfigEntry<bool> Parallax { get; set; }
        //public static ConfigEntry<bool> EnableCharacterRotation { get; set; }

        // Lights //
        // The primary light over the scene //
        public static ConfigEntry<Color> Light_Color { get; set; }

        public static ConfigEntry<bool> Light_Flicker { get; set; }
        public static ConfigEntry<float> Light_Intensity { get; set; }

        // Character Pad Displays //
        public static ConfigEntry<float> MannequinScale { get; set; }

        public static ConfigEntry<bool> MannequinEnableLocalTurn { get; set; }
        public static ConfigEntry<float> MannequinEnableLocalTurnMultiplier { get; set; }
        // public static ConfigEntry<string> MannequinLayout { get; set; }

        // Background Elements //
        // Anything in the background unrelated to the characters //
        public static ConfigEntry<bool> MeshProps { get; set; }

        public static ConfigEntry<bool> PhysicsProps { get; set; }
        public static ConfigEntry<bool> Lobby_Shaking { get; set; }

        // Custom Background //
        public static ConfigEntry<string> Scene_Selection { get; set; }

        public static ConfigEntry<bool> Scene_Header { get; set; }
        public static ConfigEntry<bool> Scene_Seer { get; set; }
        //public static ConfigEntry<EventStateType> Scene_EnableTimerStartEvent { get; set; }
        /*public enum EventStateType
        {
            AlwaysOn,
            On,
            Off
        };*/

        // Survivors In Lobby //
        // Anything related to the config setting to show displays in the lobby //
        public static ConfigEntry<bool> SIL_LockedCharactersBlack { get; set; }

        public static ConfigEntry<string> SIL_SelectedLayout { get; set; }
        public static ConfigEntry<bool> SIL_ZoomEnable { get; set; }
        public static ConfigEntry<KeyCode> SIL_ResetCameraKey { get; set; }
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

        //public static string tempMannequinName;
        public static bool tempConfirmChoice;

        #endregion tempvalues

        public static void Bind(ConfigFile config)
        {
            string cat;
            cat = "Sound";
            MusicChoice = config.Bind(cat, "Music", "auto", "Adjust: Sets the current musictrack of the lobby." +
                "\n\"default\" = Will play the default lobby music or, if the scene has one, the overrided music" +
                "\n\"auto\" = Will auto choose the scene's track." +
                "For the rest, see the readme for valid tracks.");

            // CONFIG //
            // Ordered by Layer //
            // UI //
            cat = "UI";
            UI_ShowFade = config.Bind(cat, "Show Fade", true, "Toggle: Dark fade bars at the top and bottom of the lobby.");
            UI_BlurOpacity = config.Bind(cat, "Blur Opacity", 100, "Adjust: Blur opacity behind the UI." +
                "\n0:fully transparent - 100:default");
            UI_Scale = config.Bind(cat, "UI Scale", 1f, "Adjust: Resizes the UIs on the left and right."); //def 1f
            UI_Scale.Value = Mathf.Clamp(UI_Scale.Value, UI_Scale_Min, UI_Scale_Max);

            // Overlay //
            // Anything that affects the scene as a whole //
            // Post Processing //
            cat = "Overlay";
            PostProcessing = config.Bind(cat, "Post Processing", true, "Toggle: Post processing.");
            Parallax = config.Bind(cat, "Parallax", true, "Toggle: Parallax effect on the camera controlled by the position of the cursor.");

            // Lights //
            // The primary light over the scene //
            cat = "Lights";
            Light_Color = config.Bind(cat, "Color", new Color(0.9811f, 0.3564f, 0.3564f, 1f), "Adjust: The color of the lobby's light, include # for hex values"); //#fa5a5a
            Light_Flicker = config.Bind(cat, "Flickering", true, "Toggle: Flickering of the Light.");
            Light_Intensity = config.Bind(cat, "Intensity", 0.4873f, "Adjust: Change the intensity of the light.");

            // Character Pad Displays //
            cat = "Character Display";
            MannequinScale = config.Bind(cat, "Character Display Scale", 1f, "Adjust: Resizes character displays."); //def 1f
            MannequinEnableLocalTurn = config.Bind(cat, "Character Rotate", true, "Toggle: Click and drag to rotate your character in the lobby.");
            MannequinEnableLocalTurnMultiplier = config.Bind(cat, "Character Rotate Speed", 2f, "Adjust: Sets the speed of character rotation.");
            //MannequinLayout = config.Bind(cat, "Character Display Layout", "default", "Adjust: Sets the current layout of the character displays.");

            // Background Elements //
            // Anything in the background unrelated to the characters //
            cat = "Background";
            MeshProps = config.Bind(cat, "LobbyScene: Show Static MeshProps", false, "Toggle: all the stationary meshprops.");
            PhysicsProps = config.Bind(cat, "LobbyScene: Show Physics Props", false, "Toggle: all the physics props like the Chair.");
            Lobby_Shaking = config.Bind(cat, "LobbyScene: Shaking", false, "Toggle: whether the physics objects are shaken periodically");

            // Custom Background //
            Scene_Selection = config.Bind(cat, "Select Scene", "lobby", "Adjust: Sets the current scene of the lobby.");
            Scene_Header = config.Bind(cat, "Scene Header", true, "Toggle: Shows the scene's title and subtitle.");
            Scene_Seer = config.Bind(cat, "Scene Seer", true, "Toggle: Shows the stage's seer text at the bottom.");
            /*Scene_EnableTimerStartEvent = config.Bind(cat, "Ready Events", EventStateType.On, "Adjust: Enables an event on the current Scene for when the vote starts." +
                "\nAlways On: Event starts when scene is loaded | On: Event starts when timer starts | Off: Disabled.");
            Scene_EnableTimerStartEvent.SettingChanged += Scene_EnableTimerStartEvent_SettingChanged;*/

            // Survivors In Lobby //
            // Anything related to the config setting to show displays in the lobby //
            SIL_LockedCharactersBlack = config.Bind(cat, "Enable Unavailable Shadow Survivors", true, "Toggle: Any survivors in a character layout that you don't have unlocked become shadowy.");
            SIL_SelectedLayout = config.Bind(cat, "Character Layout Name", "any_empty", "Adjust: Shows background elements in certain orientations. Set to Any_Empty to disable.");
            SIL_ZoomEnable = config.Bind(cat, "Zoom On Character Select", true, "Toggle: Selecting a character will zoom the camera onto that character.");
            SIL_ResetCameraKey = config.Bind(cat, "Reset Camera", KeyCode.Equals, "Adjust: Sets keybind for resetting the ");
            /*SelectViewMode = config.Bind("Background", "X Select View Mode (Requires SurvivorsInLobby set to true)", 0, "0 = None" +
                "\n1 = Disappear on selection" +
                "\n2 = Zoom on selection"); //def 1f*/
            //ReplayAnim = config.Bind("Background", "X Replay Animation", true, "Replays the animation for the selected character.");
            //LivePreview = config.Bind("Background", "X Live Preview", true, "Updates the appearance for the selected character.");
            SIL_ClickOnCharacterToSwap = config.Bind(cat, "Click on bg char to select (EXPERIMENTAL)", true, "Allows clicking on a character to select them." +
                "\nExperimental: Clicking on the character might be unavailable, or offset.");

            ShowLoggingText = config.Bind("zDebugging", "Print logging text to console", LoggingStyle.UserShouldSee, "If true, then some logging messages are sent to the console. Error and warning messages will still display.");

            tempSelectSceneAction += SetNewScene;
            tempSelectLayoutAction += SetNewLayout;
            //tempSelectMannequinAction += SetNewMannequin;

            tempSceneName = Scene_Selection.Value;
            tempLayoutName = SIL_SelectedLayout.Value;
        }

        public enum LoggingStyle
        {
            None,
            UserShouldSee,
            ObscureSoOnlyDevSees
        }

        public static void InLobbyBind()
        {
            inLobbyConfigEntry = new ModConfigEntry
            {
                DisplayName = "Lobby Appearance Improvements",
            };
            inLobbyConfigEntry.SectionFields["Sound"] = new List<IConfigField>
            {
                new StringConfigField(MusicChoice.Definition.Key, MusicChoice.Description.Description, () => MusicChoice.Value, null, Hook_MusicChoice)
            };
            inLobbyConfigEntry.SectionFields["UI"] = new List<IConfigField>
            {
                new BooleanConfigField(UI_ShowFade.Definition.Key, UI_ShowFade.Description.Description, () => UI_ShowFade.Value, Hook_UI_ShowFade),
                new IntConfigField(UI_BlurOpacity.Definition.Key, () => UI_BlurOpacity.Value, Hook_UI_BlurOpacity, null, 0, 100),
                new FloatConfigField(UI_Scale.Definition.Key, () => UI_Scale.Value, null, Hook_UIScale, UI_Scale_Min, UI_Scale_Max)
            };
            inLobbyConfigEntry.SectionFields["Overlay"] = new List<IConfigField>
            {
                new BooleanConfigField(PostProcessing.Definition.Key, PostProcessing.Description.Description, () => PostProcessing.Value, Hook_Overlay_ShowPostProcessing),
                new BooleanConfigField(Parallax.Definition.Key, Parallax.Description.Description, () => Parallax.Value, Hook_Overlay_Parallax),
            };
            inLobbyConfigEntry.SectionFields["Lights"] = new List<IConfigField>
            {
                new ColorConfigField(Light_Color.Definition.Key, Light_Color.Description.Description, () => Light_Color.Value, Hook_LightUpdate_Color, null, false),
                new BooleanConfigField(Light_Flicker.Definition.Key, Light_Flicker.Description.Description, () => Light_Flicker.Value, Hook_LightUpdate_Flicker),
                new FloatConfigField(Light_Intensity.Definition.Key, Light_Intensity.Description.Description, () => Light_Intensity.Value, Hook_LightUpdate_Intensity)
            };
            inLobbyConfigEntry.SectionFields["Mannequins"] = new List<IConfigField>
            {
                //new StringConfigField(MannequinLayout.Definition.Key, MannequinLayout.Description.Description, () => MannequinLayout.Value, null, tempSelectMannequinAction),
                new FloatConfigField(MannequinScale.Definition.Key, MannequinScale.Description.Description, () => MannequinScale.Value, Hook_RescalePads),
                new BooleanConfigField(MannequinEnableLocalTurn.Definition.Key, MannequinEnableLocalTurn.Description.Description, () => MannequinEnableLocalTurn.Value, Hook_Rotate_Toggle),
                new FloatConfigField(MannequinEnableLocalTurnMultiplier.Definition.Key, MannequinEnableLocalTurnMultiplier.Description.Description, () => MannequinEnableLocalTurnMultiplier.Value, Hook_Rotate_Speed),
            };
            inLobbyConfigEntry.SectionFields["Scenes+Layouts"] = new List<IConfigField>
            {
                //new SelectListField<string>(SelectedScene.Definition.Key, SelectedScene.Description.Description, SceneMethods.GetScenes, null, null, null),
                new BooleanConfigField(Scene_Header.Definition.Key, Scene_Header.Description.Description, () => Scene_Header.Value, Hook_ToggleSceneHeaderVisibility),
                new BooleanConfigField(Scene_Seer.Definition.Key, Scene_Seer.Description.Description, () => Scene_Seer.Value, Hook_ToggleSceneSeerVisibility),
                new StringConfigField(Scene_Selection.Definition.Key, Scene_Selection.Description.Description, () => Scene_Selection.Value, null, tempSelectSceneAction),
                //new EnumConfigField<EventStateType>(Scene_EnableTimerStartEvent.Definition.Key, Scene_EnableTimerStartEvent.Description.Description, () => Scene_EnableTimerStartEvent.Value, Hook_AdjustVoteStartEvent),
                new StringConfigField(SIL_SelectedLayout.Definition.Key, SIL_SelectedLayout.Description.Description, () => SIL_SelectedLayout.Value, null, tempSelectLayoutAction),
                new BooleanConfigField("Confirm Choice", "Click to confirm choice for scene.", () => tempConfirmChoice, SetSceneLayoutFromLobby),
                new BooleanConfigField(SIL_ZoomEnable.Definition.Key, SIL_ZoomEnable.Description.Description, () => SIL_ZoomEnable.Value, Hook_ToggleZooming),
                new EnumConfigField<KeyCode>(SIL_ResetCameraKey.Definition.Key, SIL_ResetCameraKey.Description.Description, () => SIL_ResetCameraKey.Value),
                new BooleanConfigField(SIL_LockedCharactersBlack.Definition.Key, SIL_LockedCharactersBlack.Description.Description, () => SIL_LockedCharactersBlack.Value, Hook_BlackenSurvivors),
            };
            inLobbyConfigEntry.SectionFields["Scene: Lobby"] = new List<IConfigField>
            {
                new BooleanConfigField(MeshProps.Definition.Key, MeshProps.Description.Description, () => MeshProps.Value, Hook_Lobby_HideProps),
                new BooleanConfigField(PhysicsProps.Definition.Key, PhysicsProps.Description.Description, () => PhysicsProps.Value, Hook_Lobby_HidePhysicsProps),
                new BooleanConfigField(Lobby_Shaking.Definition.Key, Lobby_Shaking.Description.Description, () => Lobby_Shaking.Value, Hook_Lobby_DisableShaking),
            };
            ModConfigCatalog.Add(inLobbyConfigEntry);
        }

        public static Action<string> tempSelectSceneAction;
        public static Action<string> tempSelectLayoutAction;
        //public static Action<string> tempSelectMannequinAction;

        public static void SetNewScene(string value)
        {
            tempSceneName = value;
        }

        public static void SetNewLayout(string value)
        {
            tempLayoutName = value;
        }

        public static void SetNewMannequin(string value)
        {
            //tempMannequinName = value;
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
                        Scene_Selection.Value = tempSceneName;
                        break;

                    default:
                        Scene_Selection.Value = tempSceneName;
                        SIL_SelectedLayout.Value = tempLayoutName;
                        break;
                }
            }
        }
    }
}