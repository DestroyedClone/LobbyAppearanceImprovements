using System;

namespace LobbyAppearanceImprovements
{
    public static class Actions
    {
        // CONFIG //
        // Ordered by Layer //
        // UI //
        public static Action<bool> UI_HideFade { get; set; }

        public static Action<int> UI_BlurOpacity { get; set; }
        public static Action<float> UI_Scale { get; set; }

        // Overlay //
        // Anything that affects the scene as a whole //
        // Post Processing //
        public static Action<bool> PostProcessing { get; set; }

        // Lights //
        // The primary light over the scene //
        public static Action<string> Light_Color { get; set; }

        public static Action<bool> Light_Flicker_Disable { get; set; }
        public static Action<float> Light_Intensity { get; set; }

        // Character Pad Displays //
        public static Action<float> CharacterPadScale { get; set; }

        // Background Elements //
        // Anything in the background unrelated to the characters //
        public static Action<bool> MeshProps { get; set; }

        public static Action<bool> PhysicsProps { get; set; }
        public static Action<bool> DisableShaking { get; set; }

        // Custom Background //
        public static Action<string> SelectedScene { get; set; }

        // Survivors In Lobby //
        // Anything related to the config setting to show displays in the lobby //
        public static Action<bool> SIL_Enabled { get; set; }

        public static Action<bool> SIL_LockedCharactersBlack { get; set; }
        public static Action<string> SIL_SelectedLayout { get; set; }
        public static Action<int> SelectViewMode { get; set; }
        public static Action<bool> SIL_ClickOnCharacterToSwap { get; set; }
        public static Action<bool> ReplayAnim { get; set; }
        public static Action<bool> LivePreview { get; set; }
    }
}