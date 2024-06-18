using LobbyAppearanceImprovements.Scenes;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace LobbyAppearanceImprovements
{
    internal static class LAIMusicManager
    {
        public static MusicController musicController;
        public static MusicTrackOverride musicTrackOverride;
        public static void Init()
        {
            On.RoR2.MusicController.Start += MusicController_Start;
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
            LAILogging.LogMessage($"Changing music track to \"{choice}\"", ConfigSetup.LoggingStyle.UserShouldSee);
            if (choice.ToLower() == "default")
            {
                musicTrackOverride.enabled = false;
            }
            else if (choice.ToLower() == "auto")
            {
                musicTrackOverride.enabled = true;
                LAILogging.LogMessage($"Changing music track to {scene}'s \"{scene.MusicTrackName}\"", ConfigSetup.LoggingStyle.UserShouldSee);
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
                    LAILogging.LogWarning("Couldn't find music choice \"{choice}\", defaulting...", ConfigSetup.LoggingStyle.UserShouldSee);
                    musicTrackOverride.enabled = false;
                    return;
                }
                musicTrackOverride.track = musicTrack;
            }
        }


        private static void MusicController_Start(On.RoR2.MusicController.orig_Start orig, RoR2.MusicController self)
        {
            orig(self);
            musicController = self;
        }
    }
}
