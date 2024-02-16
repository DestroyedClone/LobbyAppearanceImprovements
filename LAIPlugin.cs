using BepInEx;
using LobbyAppearanceImprovements.CharacterSceneLayouts;
using LobbyAppearanceImprovements.Scenes;
using R2API.Utils;
using RoR2.UI;
using System;
using System.Reflection;
using UnityEngine;
using static LobbyAppearanceImprovements.ConfigSetup;
using static LobbyAppearanceImprovements.HookMethods;

[assembly: HG.Reflection.SearchableAttribute.OptIn]

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

        //public static GameObject glassArtifact = Resources.Load<GameObject>("prefabs/pickupmodels/artifacts/PickupGlass");
        //public static GameObject cubeObject = glassArtifact.transform.Find("mdlArtifactSimpleCube").gameObject;

        public static Transform CharSelUITransform;
        public static Transform LAITitleRef;

        public static CharacterSelectController CharacterSelectController
        {
            get
            {
                if (!_characterSelectController)
                    _characterSelectController = UnityEngine.Object.FindObjectOfType<CharacterSelectController>();
                return _characterSelectController;
            }
            set { _characterSelectController = value; }
        }

        private static CharacterSelectController _characterSelectController;

        public void Awake()
        {
            LAILogging.Init(Logger);

            //DefaultTextObject = CreateDefaultTextObject();
            ConfigSetup.Initialize(Config);
            LAILanguage.Init();
            LAIMannequinManager.Init();
            LAICameraManager.Init();
            //AssemblySetup();

            On.RoR2.UI.CharacterSelectController.Awake += CharacterSelectController_Awake;
            LAISceneManager.Initialize();

            On.RoR2.UI.MainMenu.MainMenuController.Start += DeferredAssemblySetup;
        }

        private void DeferredAssemblySetup(On.RoR2.UI.MainMenu.MainMenuController.orig_Start orig, RoR2.UI.MainMenu.MainMenuController self)
        {
            orig(self);
            AssemblySetup();
            On.RoR2.UI.MainMenu.MainMenuController.Start -= DeferredAssemblySetup;
        }

        private void CharacterSelectController_Awake(On.RoR2.UI.CharacterSelectController.orig_Awake orig, RoR2.UI.CharacterSelectController self)
        {
            orig(self);
            CharacterSelectController = self;
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
            CharSelUITransform = GameObject.Find("CharacterSelectUI").transform;

            Methods.LoadSceneAndLayout(Scene_Selection.Value, SIL_SelectedLayout.Value);

            LAITitleRef = self.activeSurvivorInfoPanel.transform.Find("SurvivorNamePanel/SurvivorName");

            // UI //
            Hook_UI_ShowFade(UI_ShowFade.Value);
            Hook_UI_BlurOpacity(UI_BlurOpacity.Value);
            Hook_UIScale(UI_Scale.Value);

            // Overlay //
            // Post Processing //
            Hook_Overlay_Parallax(Parallax.Value);

            // Character Pad Displays //
            Hook_RescalePads(MannequinScale.Value);
            Hook_Rotate_Toggle(MannequinEnableLocalTurn.Value);
            Hook_Rotate_Speed(MannequinEnableLocalTurnMultiplier.Value);

            HookMethods.Hook_ToggleZooming(ConfigSetup.SIL_ZoomEnable.Value);
            HookMethods.Hook_DisableShaking(ConfigSetup.Shaking.Value);
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
                    if (!canLoadScene)
                        continue;
                    var sceneName = type.Name.ToLower();
                    LAISceneManager.scenesDict[sceneName] = type;
                    LAISceneManager.sceneNameList.Add(sceneName);
                    sceneObjectInitializer.Init();
                }
                else if (layoutType.IsAssignableFrom(type))
                {
                    var sceneObjectInitializer = (CharSceneLayout)Activator.CreateInstance(type);
                    bool canLoadLayout = sceneObjectInitializer.CanLoadLayout();
                    if (!canLoadLayout)
                        continue;
                    var layoutNameLower = type.Name.ToLower();
                    LAILayoutManager.layoutsDict[layoutNameLower] = type;
                    LAILayoutManager.layoutNameList.Add(layoutNameLower);
                    LAILogging.LogMessage("Initializing Scene: " + type, LoggingStyle.ObscureSoOnlyDevSees);
                    sceneObjectInitializer.Init();
                }
            }
        }
    }
}