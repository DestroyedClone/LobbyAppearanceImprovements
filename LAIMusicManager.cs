﻿using LobbyAppearanceImprovements.Scenes;
using RoR2;

namespace LobbyAppearanceImprovements
{
    internal static class LAIMusicManager
    {
        public static MusicTrackOverride musicTrackOverride;

        public static void Init()
        {
            On.RoR2.UI.CharacterSelectController.Awake += CharacterSelectController_Awake;
        }

        private static void CharacterSelectController_Awake(On.RoR2.UI.CharacterSelectController.orig_Awake orig, RoR2.UI.CharacterSelectController self)
        {
            musicTrackOverride = self.gameObject.AddComponent<MusicTrackOverride>();
            musicTrackOverride.priority = 99999;
            musicTrackOverride.track = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<MusicTrackDef>("RoR2/Base/Common/muLogbook").WaitForCompletion();
            musicTrackOverride.enabled = false;
            orig(self);
        }

        public static void OnSceneLoaded(LAIScene scene = null)
        {
            var choice = ConfigSetup.MusicChoice.Value;
            scene ??= LAISceneManager.chosenScene;
            LAILogging.LogMessage($"Changing music track to \"{choice}\"", ConfigSetup.LoggingStyle.ObscureSoOnlyDevSees);
            if (choice.ToLower() == "default")
            {
                musicTrackOverride.enabled = false;
            }
            else if (choice.ToLower() == "auto")
            {
                musicTrackOverride.enabled = true;
                LAILogging.LogMessage($"Changing music track to {scene}'s \"{scene.MusicTrackName}\"", ConfigSetup.LoggingStyle.ObscureSoOnlyDevSees);
                var musicTrack = MusicTrackCatalog.FindMusicTrackDef(scene.MusicTrackName);
                if (musicTrack == null) musicTrackOverride.enabled = false;
                musicTrackOverride.track = musicTrack;
            }
            else
            {
                musicTrackOverride.enabled = true;
                var musicTrack = MusicTrackCatalog.FindMusicTrackDef(choice);
                if (musicTrack == null)
                {
                    //musicTrack = LAIScene.defaultMusicTrackDef;
                    LAILogging.LogWarning("Couldn't find music choice \"{choice}\", defaulting...", ConfigSetup.LoggingStyle.ObscureSoOnlyDevSees);
                    musicTrackOverride.enabled = false;
                    return;
                }
                musicTrackOverride.track = musicTrack;
            }
        }
    }
}