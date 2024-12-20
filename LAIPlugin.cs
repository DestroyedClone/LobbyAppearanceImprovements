﻿using BepInEx;
using LobbyAppearanceImprovements.Layouts;
using LobbyAppearanceImprovements.MannequinLayouts;
using LobbyAppearanceImprovements.Scenes;
using R2API.Utils;
using RoR2;
using RoR2.UI;
using System;
using System.Collections;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using static LobbyAppearanceImprovements.ConfigSetup;
using static LobbyAppearanceImprovements.HookMethods;

[assembly: HG.Reflection.SearchableAttribute.OptIn]

namespace LobbyAppearanceImprovements
{
    [BepInDependency(R2API.R2API.PluginGUID, R2API.R2API.PluginVersion)]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    [BepInPlugin(ModGuid, ModName, ModVer)]
    [BepInDependency("com.KingEnderBrine.InLobbyConfig")]

    //Scene Dependencies
    //PaladinOnly
        [BepInDependency("com.rob.Paladin", BepInDependency.DependencyFlags.SoftDependency)]
    public partial class LAIPlugin : BaseUnityPlugin
    {
        public const string ModVer = "1.3.0";
        public const string ModName = "LobbyAppearanceImprovements";
        public const string ModGuid = "com.DestroyedClone.LobbyAppearanceImprovements";

        public static Transform CharSelUITransform;
        public static Transform LAITitleRef;
        public static GameObject LAITextHolder;

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

            StartCoroutine(CallInit());

            ConfigSetup.Initialize(Config);
            LAIMannequinManager.Init();
            LAICameraManager.Init();
            LAIMusicManager.Init();
            LAIPatches.Init();

            On.RoR2.UI.CharacterSelectController.Awake += CharacterSelectController_Awake;
            LAISceneManager.Initialize();

            RoR2Application.onLoad += AssemblySetup;

            //RoR2.Stage.onServerStageBegin += CacheSkyboxMaterial;
            On.RoR2.PreGameShakeController.Awake += PreGameShakeController_Awake;
        }
        private IEnumerator CallInit()
        {
            var initTask = LAIAssets.Init(); // Start async method
            while (!initTask.IsCompleted) // Wait until task is completed
            {
                yield return null; // Continue waiting on next frame
            }

            if (initTask.Exception != null)
            {
                Debug.LogError("Exception in LAIAssets.Init: " + initTask.Exception);
            }
        }

        private void PreGameShakeController_Awake(On.RoR2.PreGameShakeController.orig_Awake orig, PreGameShakeController self)
        {
            InstanceTracker.Add(this);
            self.gameObject.SetActive(ConfigSetup.Lobby_Shaking.Value);
            orig(self);
        }

        public static StringBuilder stringBuilder = new();

        private void CacheSkyboxMaterial(RoR2.Stage obj)
        {
            stringBuilder.AppendLine($"{obj.sceneDef.cachedName} - {RenderSettings.skybox} - {RenderSettings.skybox.name}");
            Debug.Log(stringBuilder.ToString());
        }

        private void CharacterSelectController_Awake(On.RoR2.UI.CharacterSelectController.orig_Awake orig, RoR2.UI.CharacterSelectController self)
        {
            orig(self);
            if (Run.instance) return;
            if (UnityEngine.SceneManagement.SceneManager.sceneCount == 1)
            {
                //_logger.LogMessage("good");
            }
            else
            {
                //_logger.LogMessage("Preventing activating stuff");
                return;
            }
            CharacterSelectController = self;
            if (!self.gameObject.GetComponent<Methods.LAICameraController>())
                self.gameObject.AddComponent<Methods.LAICameraController>();
            CharSelUITransform = self.transform;

            if (!LAITextHolder)
                LAITextHolder = new GameObject("LAI_TextHolder");

            LAITextHolder.transform.parent = self.transform.Find("SafeArea");
            //var csf = LAITextHolder.gameObject.AddComponent<ContentSizeFitter>();
            //var vlg = LAITextHolder.gameObject.AddComponent<VerticalLayoutGroup>();
            LAITextHolder.transform.localPosition = new (100, 0, 0);
            //LAITextHolder.gameObject.AddComponent<Image>();

            LAITitleRef = self.activeSurvivorInfoPanel.transform.Find("SurvivorNamePanel/SurvivorName");

            ValidateConfig();
            Methods.LoadSceneAndLayout(Scene_Selection.Value, SIL_SelectedLayout.Value);

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
            HookMethods.Hook_Lobby_DisableShaking(ConfigSetup.Lobby_Shaking.Value);
        }

        private void ValidateConfig()
        {
            if (!LAISceneManager.sceneNameList.Contains(ConfigSetup.Scene_Selection.Value))
            {
                LAILogging.LogWarning($"Invalid scene name: {ConfigSetup.Scene_Selection.Value}, switching to {ConfigSetup.Scene_Selection.DefaultValue}", LoggingStyle.UserShouldSee);
                ConfigSetup.Scene_Selection.Value = (string)ConfigSetup.Scene_Selection.DefaultValue;
            }
            if (!LAILayoutManager.layoutNameList.Contains(ConfigSetup.SIL_SelectedLayout.Value))
            {
                LAILogging.LogWarning($"Invalid layout name: {ConfigSetup.SIL_SelectedLayout.Value}, switching to {ConfigSetup.SIL_SelectedLayout.DefaultValue}", LoggingStyle.UserShouldSee);
                ConfigSetup.SIL_SelectedLayout.Value = (string)ConfigSetup.SIL_SelectedLayout.DefaultValue;
            }
        }

        //Defer?
        public void AssemblySetup() //credit to bubbet for base code
        {
            var sceneType = typeof(LAIScene);
            var layoutType = typeof(LAILayout);
            var mannequinType = typeof(BaseMannequinLayout);
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
                    LAISceneManager.scenesReverseDict[type] = name;
                    LAISceneManager.sceneNameList.Add(sceneName);
                    sceneObjectInitializer.Init();
                }
                else if (layoutType.IsAssignableFrom(type))
                {
                    var sceneObjectInitializer = (LAILayout)Activator.CreateInstance(type);
                    bool canLoadLayout = sceneObjectInitializer.CanLoadLayout();
                    if (!canLoadLayout)
                        continue;
                    var layoutNameLower = type.Name.ToLower();
                    LAILayoutManager.layoutsDict[layoutNameLower] = type;
                    LAILayoutManager.layoutNameList.Add(layoutNameLower);
                    LAILogging.LogMessage("Initializing Scene: " + type, LoggingStyle.ObscureSoOnlyDevSees);
                    sceneObjectInitializer.Init();
                }
                /*else if (mannequinType.IsAssignableFrom(type))
                {
                    var sceneObjectInitializer = (BaseMannequinLayout)Activator.CreateInstance(type);
                    //bool canLoadMannequin = sceneObjectInitializer.CanLoadLayout();
                    //if (!canLoadMannequin)
                       // continue;
                    var layoutNameLower = type.Name.ToLower();
                    LAIMannequinManager.mannequinLayoutsDict[layoutNameLower] = type;
                    LAIMannequinManager.mannequinLayoutNameList.Add(layoutNameLower);
                    LAILogging.LogMessage("Initializing Mannequin Layout: " + type, LoggingStyle.ObscureSoOnlyDevSees);
                    sceneObjectInitializer.Init();
                }*/
            }
        }
    }
}