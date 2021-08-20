using BepInEx.Configuration;

namespace LobbyAppearanceImprovements
{
    public static class ConfigSetup
    {
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

        public static ConfigEntry<string> SelectedLayout { get; set; }
        public static ConfigEntry<int> SelectViewMode { get; set; }
        public static ConfigEntry<bool> ReplayAnim { get; set; }
        public static ConfigEntry<bool> LivePreview { get; set; }

        #endregion Config

        public static void Bind(ConfigFile config)
        {
            // CONFIG //
            // Ordered by Layer //
            // UI //
            HideFade = config.Bind("UI", "Hide Fade", true, "There's a dark fade on the top and bottom, this disables it.");
            BlurValue = config.Bind("UI", "Adjust Blur (Not Implemented)", 255, "Adjusts the blur behind the UI elements on the left and right." +
                "\n0:fully transparent - 255:default");
            UIScale = config.Bind("UI", "UI Scale", 1f, "Resizes the UIs on the left and right."); //def 1f

            // Overlay //
            // Anything that affects the scene as a whole //
            // Post Processing //
            PostProcessing = config.Bind("UI", "Disable Post Processing", true, "Disables the blurry post processing.");

            // Lights //
            // The primary light over the scene //
            Light_Color = config.Bind("Lights", "Hex Color", "default", "Change the default color of the light"); //#fa5a5a
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
            SurvivorsInLobby = config.Bind("Background", "Survivors In Lobby", true, "Shows survivors in the lobby." +
                "\nThese background survivors don't reflect the loadouts in the lobby.");
            SelectedLayout = config.Bind("Background", "Survivors In Lobby Layout", "default", "Layout of the survivors in the scene.");
            SelectViewMode = config.Bind("Other", "X Select View Mode (Requires SurvivorsInLobby set to true)", 0, "0 = None" +
                "\n1 = Disappear on selection" +
                "\n2 = Zoom on selection"); //def 1f
            ReplayAnim = config.Bind("Background", "X Replay Animation", true, "Replays the animation for the selected character.");
            LivePreview = config.Bind("Background", "X Live Preview", true, "Updates the appearance for the selected character.");
        }
    }
}