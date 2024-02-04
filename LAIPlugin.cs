using BepInEx;
using LobbyAppearanceImprovements.CharacterSceneLayouts;
using LobbyAppearanceImprovements.Scenes;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.SurvivorMannequins;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using static LobbyAppearanceImprovements.ConfigSetup;
using static LobbyAppearanceImprovements.HookMethods;

namespace LobbyAppearanceImprovements
{
    [BepInDependency(R2API.R2API.PluginGUID, R2API.R2API.PluginVersion)]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    [BepInPlugin(ModGuid, ModName, ModVer)]
    [BepInDependency("com.KingEnderBrine.InLobbyConfig")]

    //Scene Depedencies
    //PaladinOnly
    [BepInDependency("com.rob.Paladin", BepInDependency.DependencyFlags.SoftDependency)]

    //Sniper Layout
    //[BepInDependency("com.Moffein.SniperClassic", BepInDependency.DependencyFlags.SoftDependency)]
    //[BepInDependency("Rein.Sniper", BepInDependency.DependencyFlags.SoftDependency)]
    [R2APISubmoduleDependency(nameof(R2API.SceneAssetAPI))]
    public partial class LAIPlugin : BaseUnityPlugin
    {
        public const string ModVer = "1.2.0";
        public const string ModName = "LobbyAppearanceImprovements";
        public const string ModGuid = "com.DestroyedClone.LobbyAppearanceImprovements";

        internal static BepInEx.Logging.ManualLogSource _logger = null;
        public static void LogMessage(string message, ConfigSetup.LoggingStyle loggingStyle)
        {
            if (ConfigSetup.ShowLoggingText.Value >= loggingStyle)
                _logger.LogMessage(message);
        }
        public static void LogWarning(string message, ConfigSetup.LoggingStyle loggingStyle)
        {
            if (ConfigSetup.ShowLoggingText.Value >= loggingStyle)
                _logger.LogWarning(message);
        }
        public static void LogError(string message, ConfigSetup.LoggingStyle loggingStyle)
        {
            if (ConfigSetup.ShowLoggingText.Value >= loggingStyle)
                _logger.LogError(message);
        }

        public static LAIScene chosenScene = null;
        public static Dictionary<string, Type> scenesDict = new Dictionary<string, Type>();
        public static List<string> sceneNameList = new List<string>();
        public static GameObject sceneInstance;

        public static CharSceneLayout chosenLayout = null;
        public static Dictionary<string, Type> layoutsDict = new Dictionary<string, Type>();
        public static List<string> layoutNameList = new List<string>();
        public static GameObject layoutInstance;

        public static Dictionary<string, CharSceneLayout.CameraSetting> currentCameraSettings = new Dictionary<string, CharSceneLayout.CameraSetting>();

        //public static GameObject glassArtifact = Resources.Load<GameObject>("prefabs/pickupmodels/artifacts/PickupGlass");
        //public static GameObject cubeObject = glassArtifact.transform.Find("mdlArtifactSimpleCube").gameObject;

        public static GameObject MeshPropsRef;
        public static Transform UI_OriginRef;
        public static Transform TitleRef;
        public static CharacterSelectController characterSelectController = null;
        public static SurvivorMannequinDioramaController mannequinDioramaController = null;

        //public static GameObject DefaultTextObject;

        public static Methods.LAICameraController CurrentCameraController;

        public void Awake()
        {
            _logger = Logger;

            //DefaultTextObject = CreateDefaultTextObject();
            ConfigSetup.Bind(Config);
            ConfigSetup.InLobbyBind();
            CommandHelper.AddToConsoleWhenReady();
            LAILanguage.Init();
            //AssemblySetup();

            On.RoR2.UI.CharacterSelectController.Awake += CharacterSelectController_Awake;
            On.RoR2.SurvivorMannequins.SurvivorMannequinDioramaController.OnEnable += (orig, self) =>
            {
                mannequinDioramaController = self;
                orig(self);
            };
            // Hook Start instead?

            SceneSetup.Init();

            LAIScene.onSceneLoaded += OnSceneLoaded;

            On.RoR2.CameraRigController.Start += CameraRigController_Start;
            On.RoR2.UI.MainMenu.MainMenuController.Start += DeferredSceneLayoutSetup;
            On.RoR2.PreGameController.RefreshLobbyBackground += PreGameController_RefreshLobbyBackground;


        }

        public static void OnSceneLoaded(LAIScene laiScene)
        {
            if (LAIPlugin.sceneInstance && LAIPlugin.sceneInstance.transform.Find("MeshProps"))
                MeshPropsRef = LAIPlugin.sceneInstance.transform.Find("MeshProps").gameObject;
        }

        private void PreGameController_RefreshLobbyBackground(On.RoR2.PreGameController.orig_RefreshLobbyBackground orig, PreGameController self)
        {
            orig(self);
            if (self.lobbyBackground) self.lobbyBackground.SetActive(false);
        }

        private void DeferredSceneLayoutSetup(On.RoR2.UI.MainMenu.MainMenuController.orig_Start orig, RoR2.UI.MainMenu.MainMenuController self)
        {
            orig(self);
            AssemblySetup();
            On.RoR2.UI.MainMenu.MainMenuController.Start -= DeferredSceneLayoutSetup;
        }

        private void CameraRigController_Start(On.RoR2.CameraRigController.orig_Start orig, CameraRigController self)
        {
            orig(self);
            var a = self.gameObject.AddComponent<CameraController>();
            a.SetCam(self);
            a.enabled = false;
        }

        public static GameObject[] CreateDefaultText()
        {
            List<GameObject> gameObjects = new List<GameObject>();
            var cocks = GameObject.Find("CharacterSelectUI/SafeArea/ReadyPanel/ReadyButton/ReadyText");
            gameObjects.Add(cocks.gameObject);
            return gameObjects.ToArray();
        }

        public static GameObject CreateDefaultTextObject()
        {
            var textPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/effects/DamageRejected"), "DeathMessageAboveCorpse_DefaultTextObjectChild");
            textPrefab.name = "DeathMessageAboveCorpse_DefaultTextObject";
            UnityEngine.Object.Destroy(textPrefab.GetComponent<EffectComponent>());
            UnityEngine.Object.Destroy(textPrefab.GetComponent<ObjectScaleCurve>());
            UnityEngine.Object.Destroy(textPrefab.GetComponent<VelocityRandomOnStart>());
            UnityEngine.Object.Destroy(textPrefab.GetComponent<ConstantForce>());
            UnityEngine.Object.Destroy(textPrefab.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(textPrefab.GetComponent<DestroyOnTimer>());
            UnityEngine.Object.Destroy(textPrefab.transform.Find("TextMeshPro").gameObject.GetComponent<ScaleSpriteByCamDistance>());
            UnityEngine.Object.Destroy(textPrefab.transform.Find("TextMeshPro").gameObject.GetComponent<Billboard>());

            TextObjectComponent textObjectComponent = textPrefab.AddComponent<TextObjectComponent>();
            textObjectComponent.textMeshPro = textPrefab.transform.Find("TextMeshPro").gameObject.GetComponent<TextMeshPro>();
            textObjectComponent.languageTextMeshController = textPrefab.transform.Find("TextMeshPro").gameObject.GetComponent<LanguageTextMeshController>();
            return textPrefab;
        }

        public class TextObjectComponent : MonoBehaviour
        {
            public TextMeshPro textMeshPro;
            public LanguageTextMeshController languageTextMeshController;
        }

        private void CharacterSelectController_Awake(On.RoR2.UI.CharacterSelectController.orig_Awake orig, RoR2.UI.CharacterSelectController self)
        {
            orig(self);
            characterSelectController = self;
            if (UnityEngine.SceneManagement.SceneManager.sceneCount == 1)
            {
                //_logger.LogMessage("good");
            }
            else
            {
                //_logger.LogMessage("Preventing activating stuff");
                return;
            }
            if (!self.gameObject.GetComponent<Methods.LAICameraController>())
                self.gameObject.AddComponent<Methods.LAICameraController>();
            UI_OriginRef = GameObject.Find("CharacterSelectUI").transform;

            Methods.LoadSceneAndLayout(Scene_Selection.Value, SIL_SelectedLayout.Value);

            TitleRef = self.activeSurvivorInfoPanel.transform.Find("SurvivorNamePanel/SurvivorName");

            // UI //
            Hook_UI_ShowFade(UI_ShowFade.Value);
            Hook_UI_BlurOpacity(UI_BlurOpacity.Value);
            Hook_UIScale(UI_Scale.Value);

            // Overlay //
            // Post Processing //
            Hook_Overlay_ShowPostProcessing(PostProcessing.Value);
            Hook_Overlay_Parallax(Parallax.Value);

            // Lights //
            Hook_LightUpdate_Color(Light_Color.Value);
            Hook_LightUpdate_Flicker(Light_Flicker.Value);
            Hook_LightUpdate_Intensity(Light_Intensity.Value);

            // Character Pad Displays //
            Hook_RescalePads(MannequinScale.Value);
            Hook_Rotate_Toggle(MannequinEnableLocalTurn.Value);
            Hook_Rotate_Speed(MannequinEnableLocalTurnMultiplier.Value);

            // Background Elements //
            Hook_HideProps(MeshProps.Value);
            Hook_HidePhysicsProps(PhysicsProps.Value);
            Hook_DisableShaking(Shaking.Value);
            Hook_ToggleZooming(SIL_ZoomEnable.Value);
        }

        //Defer?
        public void AssemblySetup() //credit to bubbet for base code
        {
            var sceneType = typeof(LAIScene);
            var layoutType = typeof(CharSceneLayout);
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.IsAbstract)
                {
                    continue;
                }
                if (sceneType.IsAssignableFrom(type))
                {
                    var sceneObjectInitializer = (LAIScene)Activator.CreateInstance(type);
                    bool canLoadScene = sceneObjectInitializer.CanLoadScene();
                    if (canLoadScene)
                    {
                        var sceneName = type.Name.ToLower();
                        scenesDict[sceneName] = type;
                        sceneNameList.Add(sceneName);
                    }
                }
                else if (layoutType.IsAssignableFrom(type))
                {
                    var sceneObjectInitializer = (CharSceneLayout)Activator.CreateInstance(type);
                    bool canLoadLayout = sceneObjectInitializer.CanLoadLayout();
                    if (canLoadLayout)
                    {
                        var layoutNameLower = type.Name.ToLower();
                        layoutsDict[layoutNameLower] = type;
                        layoutNameList.Add(layoutNameLower);
                        //var selectedLayout = layoutsDict.TryGetValue(type.Name, out var layout);
                        LAIPlugin.LogMessage("Initializing Scene: " + type, LoggingStyle.Developer);
                        sceneObjectInitializer.Init();
                    }
                }
            }
        }
    }
}