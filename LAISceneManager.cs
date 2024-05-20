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
        public static Dictionary<string, Type> scenesDict = new Dictionary<string, Type>();
        public static List<string> sceneNameList = new List<string>();
        public static GameObject sceneInstance;

        public static GameObject TitleInstance;
        public static GameObject SubTitleInstance;
        public static GameObject LayoutTitleInstance;

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
            LAILayout.onLayoutLoaded += OnLayoutLoaded;
            //On.RoR2.UI.CharacterSelectController.ClientSetReady += CharacterSelectController_ClientSetReady;
            On.RoR2.PreGameController.Start += PreGameController_Start;
            LAIScene.onSceneLoaded += ActivateVoteStartEffectIfNewSceneLoaded;
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
            HookMethods.Hook_DisableShaking(ConfigSetup.Shaking.Value);
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

        //disgusting
        private class TitleRefEnsurer : MonoBehaviour
        {
            public bool foundRef = false;
            public float stopwatch;
            private const float cooldown = 1f;

            private void Awake()
            {
                stopwatch = cooldown;
            }

            private void FixedUpdate()
            {
                stopwatch -= Time.fixedDeltaTime;
                if (stopwatch < 0)
                {
                    stopwatch = cooldown;
                    var refCheck = LAIPlugin.CharacterSelectController.transform.Find("SurvivorNamePanel/SurvivorName");
                    if (refCheck)
                        LAIPlugin.LAITitleRef = refCheck;
                    if (LAIPlugin.LAITitleRef)
                    {
                        LAILogging.LogMessage($"Found the titleref!", LoggingStyle.UserShouldSee);
                        CreateOrUpdateHeaderText();
                        enabled = false;
                        Destroy(gameObject);
                    }
                }
            }
        }

        public static void CreateOrUpdateHeaderText()
        {
            if (!LAIPlugin.LAITitleRef)
            {
                LAILogging.LogMessage($"LAITITLEREF missing, attempting spawn", LoggingStyle.UserShouldSee);
                new GameObject("LAI_TEMP_ENSUREHEADERREF").AddComponent<TitleRefEnsurer>();
                return;
            }
            LAILogging.LogMessage($"LAITITLEREF existing!, attempting spawn", LoggingStyle.UserShouldSee);
            if (!TitleInstance)
            {
                LAISceneManager.TitleInstance = UnityEngine.Object.Instantiate(LAIPlugin.LAITitleRef.gameObject, LAIPlugin.CharacterSelectController.transform);
                LAISceneManager.TitleInstance.name = $"LobbyAppearanceImprovements_Scene_Title";
                LAISceneManager.TitleInstance.transform.localPosition = new Vector3(0f, 450f, 0f);
                //TitleInstance.GetComponent<HGTextMeshProUGUI>().alpha = 0f;
                //TitleInstance.GetComponent<HGTextMeshProUGUI>().CrossFadeAlpha(1f, 3f, false);
            }
            LAISceneManager.TitleInstance.GetComponent<HGTextMeshProUGUI>().text = RoR2.Language.GetString(LAISceneManager.chosenScene.SceneTitleToken);
            if (!SubTitleInstance)
            {
                LAISceneManager.SubTitleInstance = UnityEngine.Object.Instantiate(LAIPlugin.LAITitleRef.gameObject, LAIPlugin.CharacterSelectController.transform);
                LAISceneManager.SubTitleInstance.name = $"LobbyAppearanceImprovements_Scene_Subtitle";
                LAISceneManager.SubTitleInstance.transform.localPosition = new Vector3(0f, 500f, 300f);
                //SubTitleInstance.GetComponent<HGTextMeshProUGUI>().alpha = 0f;
                //SubTitleInstance.GetComponent<HGTextMeshProUGUI>().CrossFadeAlpha(1f, 3f, false);
            }
            LAISceneManager.SubTitleInstance.GetComponent<HGTextMeshProUGUI>().text = RoR2.Language.GetStringFormatted("LAI_MAP_SUBTTILE_FORMAT", RoR2.Language.GetString(LAISceneManager.chosenScene.SceneSubtitleToken));
            if (!LayoutTitleInstance)
            {
                LAISceneManager.LayoutTitleInstance = UnityEngine.Object.Instantiate(LAIPlugin.LAITitleRef.gameObject, LAIPlugin.CharacterSelectController.transform);
                LAISceneManager.LayoutTitleInstance.name = $"LobbyAppearanceImprovements_Layout_Title";
                LAISceneManager.LayoutTitleInstance.transform.localPosition = new Vector3(0f, 600f, 800f);
            }
            LAISceneManager.LayoutTitleInstance.GetComponent<HGTextMeshProUGUI>().text = RoR2.Language.GetStringFormatted("LAI_MAP_LAYOUT_FORMAT", RoR2.Language.GetString(LAILayoutManager.chosenLayout.LayoutTitleToken));
            HookMethods.Hook_ToggleSceneHeaderVisibility(ConfigSetup.Scene_Header.Value);
        }
    }
}