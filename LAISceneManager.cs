using LobbyAppearanceImprovements.Layouts;
using LobbyAppearanceImprovements.Scenes;
using RoR2;
using RoR2.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using static LobbyAppearanceImprovements.ConfigSetup;

namespace LobbyAppearanceImprovements
{
    internal static class LAISceneManager
    {
        public static LAIScene chosenScene = null;
        public static string chosenSceneAsString = "None";
        public static Dictionary<string, Type> scenesDict = new Dictionary<string, Type>();
        public static Dictionary<Type, string> scenesReverseDict = new Dictionary<Type, string>();
        public static List<string> sceneNameList = new List<string>();
        public static GameObject sceneInstance;

        public static GameObject TitleInstance;
        public static GameObject SubTitleInstance;
        public static GameObject LayoutTitleInstance;
        public static GameObject SeerTextInstance;

        public static Action<LAIScene> onVoteStarted;

        public static bool VoteHasStarted = false;
        /*public static bool ShouldStartEventBeActive
        {
            get
            {
                var con = ConfigSetup.Scene_EnableTimerStartEvent.Value;
                return con == EventStateType.AlwaysOn
                    || con == EventStateType.On && VoteHasStarted;
            }
        }*/

        public static void Initialize()
        {
            SceneSetup.Init();
            On.RoR2.PreGameController.RefreshLobbyBackground += RemoveDefaultLobby;
            LAIScene.onSceneLoaded += CreateHeaderIfMissing;
            LAIScene.onSceneLoaded += OnSceneLoaded;
            //LAIScene.onSceneLoaded += CreateSeerTextOnLoad;
            LAILayout.onLayoutLoaded += OnLayoutLoaded;
            //On.RoR2.UI.CharacterSelectController.ClientSetReady += CharacterSelectController_ClientSetReady;
            On.RoR2.PreGameController.Start += PreGameController_Start;
            LAIScene.onSceneLoaded += ActivateVoteStartEffectIfNewSceneLoaded;
            LAIScene.onSceneUnloaded += DestroySeerText;
        }

        private static void DestroySeerText(LAIScene scene)
        {
            if (SeerTextInstance)
            {
                //UnityEngine.Object.Destroy(SeerTextInstance);
            }
        }

        private static void CreateSeerTextOnLoad(LAIScene scene)
        {
            if (!SeerTextInstance)
            {
                LAISceneManager.SeerTextInstance = UnityEngine.Object.Instantiate(LAIAssets.bombardierTextObject, LAIPlugin.LAITextHolder.transform);
                LAISceneManager.SeerTextInstance.name = $"LobbyAppearanceImprovements_Seer_Text";
                LAISceneManager.SeerTextInstance.transform.localPosition = new Vector3(0f, -480f, 0f);
                SeerTextInstance.GetComponent<HGTextMeshProUGUI>().fontSizeMin = 50f;
            }
            LAISceneManager.SeerTextInstance.GetComponent<HGTextMeshProUGUI>().text = RoR2.Language.GetStringFormatted("LAI_MAP_LAYOUT_FORMAT", RoR2.Language.GetString(LAISceneManager.chosenScene.SeerToken));
            HookMethods.Hook_ToggleSceneSeerVisibility(ConfigSetup.Scene_Seer.Value);
        }

        private static void ActivateVoteStartEffectIfNewSceneLoaded(LAIScene scene)
        {
            if (VoteHasStarted)
                onVoteStarted.Invoke(scene);
        }

        private static void PreGameController_Start(On.RoR2.PreGameController.orig_Start orig, PreGameController self)
        {
            orig(self);
            VoteHasStarted = false;
            self.gameObject.AddComponent<VoteStartedEventController>();
        }

        public class VoteStartedEventController : MonoBehaviour
        {
            public VoteController voteController;

            public float stopwatch;
            public float duration = 1f;

            public void Start()
            {
                voteController = PreGameController.instance.GetComponent<VoteController>();
            }

            public void FixedUpdate()
            {
                stopwatch -= Time.fixedDeltaTime;
                if (stopwatch > 0)
                {
                    return;
                }
                stopwatch = duration;
                if (!VoteHasStarted && voteController.NetworktimerIsActive)
                {
                    onVoteStarted.Invoke(chosenScene);
                }
                VoteHasStarted = voteController.NetworktimerIsActive;
            }
        }

        private static void OnSceneLoaded(LAIScene scene)
        {
            HookMethods.Hook_MusicChoice(MusicChoice.Value);
            HookMethods.Hook_Lobby_DisableShaking(ConfigSetup.Lobby_Shaking.Value);
            RenderSettings.skybox = scene.SkyboxOverride;
        }

        private static void OnLayoutLoaded(LAILayout layout)
        {
            CreateOrUpdateHeaderText();
        }

        private static void RemoveDefaultLobby(On.RoR2.PreGameController.orig_RefreshLobbyBackground orig, PreGameController self)
        {
            orig(self);
            if (self.lobbyBackground) self.lobbyBackground.SetActive(false);
        }

        private static void CreateHeaderIfMissing(LAIScene scene)
        {
            if (scene == null) return;
            CreateOrUpdateHeaderText();
        }

        public static void CreateOrUpdateHeaderText()
        {
            //LAILogging.LogMessage($"LAITITLEREF existing!, attempting spawn", LoggingStyle.UserShouldSee);
            if (!TitleInstance)
            {
                LAISceneManager.TitleInstance = UnityEngine.Object.Instantiate(LAIAssets.bombardierTextObject, LAIPlugin.LAITextHolder.transform);
                LAISceneManager.TitleInstance.name = $"LobbyAppearanceImprovements_Scene_Title";
                LAISceneManager.TitleInstance.transform.localPosition = new Vector3(0f, 450f, 0f);
                TitleInstance.GetComponent<HGTextMeshProUGUI>().fontSizeMin = 80;
                //TitleInstance.GetComponent<HGTextMeshProUGUI>().CrossFadeAlpha(1f, 3f, false);
            }
            LAISceneManager.TitleInstance.GetComponent<HGTextMeshProUGUI>().text = RoR2.Language.GetString(LAISceneManager.chosenScene.SceneTitleToken);
            if (!SubTitleInstance)
            {
                LAISceneManager.SubTitleInstance = UnityEngine.Object.Instantiate(LAIAssets.bombardierTextObject, LAIPlugin.LAITextHolder.transform);
                LAISceneManager.SubTitleInstance.name = $"LobbyAppearanceImprovements_Scene_Subtitle";
                LAISceneManager.SubTitleInstance.transform.localPosition = new Vector3(0f, 400f, 0f);
                SubTitleInstance.GetComponent<HGTextMeshProUGUI>().fontSizeMin = 50;
                //SubTitleInstance.GetComponent<HGTextMeshProUGUI>().alpha = 0f;
                //SubTitleInstance.GetComponent<HGTextMeshProUGUI>().CrossFadeAlpha(1f, 3f, false);
            }
            LAISceneManager.SubTitleInstance.GetComponent<HGTextMeshProUGUI>().text = RoR2.Language.GetStringFormatted("LAI_MAP_SUBTTILE_FORMAT", RoR2.Language.GetString(LAISceneManager.chosenScene.SceneSubtitleToken));
            if (!LayoutTitleInstance)
            {
                LAISceneManager.LayoutTitleInstance = UnityEngine.Object.Instantiate(LAIAssets.bombardierTextObject, LAIPlugin.LAITextHolder.transform);
                LAISceneManager.LayoutTitleInstance.name = $"LobbyAppearanceImprovements_Layout_Title";
                LAISceneManager.LayoutTitleInstance.transform.localPosition = new Vector3(0f, 360f, 0f);
                LayoutTitleInstance.GetComponent<HGTextMeshProUGUI>().fontSizeMin = 40f;
            }
            LAISceneManager.LayoutTitleInstance.GetComponent<HGTextMeshProUGUI>().text = RoR2.Language.GetStringFormatted("LAI_MAP_LAYOUT_FORMAT", RoR2.Language.GetString(LAILayoutManager.GetLayoutTitleToken()));
            HookMethods.Hook_ToggleSceneHeaderVisibility(ConfigSetup.Scene_Header.Value);
            CreateSeerTextOnLoad(LAISceneManager.chosenScene);
        }
    }
}