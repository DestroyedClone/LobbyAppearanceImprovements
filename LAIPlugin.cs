using BepInEx;
using LeTai.Asset.TranslucentImage;
using LobbyAppearanceImprovements.CharacterSceneLayouts;
using LobbyAppearanceImprovements.Scenes;
using R2API.Utils;
using RoR2;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using static LobbyAppearanceImprovements.ConfigSetup;
using static UnityEngine.ColorUtility;
using static LobbyAppearanceImprovements.HookMethods;

[module: UnverifiableCode]
#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
//[assembly: HG.Reflection.SearchableAttribute.OptIn]

namespace LobbyAppearanceImprovements
{
    [BepInDependency(R2API.R2API.PluginGUID, R2API.R2API.PluginVersion)]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    [BepInPlugin(ModGuid, ModName, ModVer)]
    [BepInDependency("com.rob.Paladin", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.KingEnderBrine.InLobbyConfig")]
    public class LAIPlugin : BaseUnityPlugin
    {
        public const string ModVer = "1.1.0";
        public const string ModName = "LobbyAppearanceImprovements";
        public const string ModGuid = "com.DestroyedClone.LobbyAppearanceImprovements";

        public static string[] PhysicsPropNames = new string[]
        {
            "PropAnchor", "ExtinguisherMesh", "FolderMesh", "LaptopMesh (1)", "ChairPropAnchor", "ChairMesh",
                    "ChairWeight","PropAnchor (1)","ExtinguisherMesh (1)","ExtinguisherMesh (2)", "FolderMesh (1)", "LaptopMesh (2)"
        };

        public static string[] MeshPropNames = new string[]
        {
            "HANDTeaser", "HumanCrate1Mesh", "HumanCrate2Mesh", "HumanCanister1Mesh"
        };

        public static LAIScene chosenScene = null;
        public static Dictionary<string, Type> scenesDict = new Dictionary<string, Type>();
        public static List<string> sceneNameList = new List<string>();
        public static GameObject sceneInstance;

        public static CharSceneLayout chosenLayout = null;
        public static Dictionary<string, Type> layoutsDict = new Dictionary<string, Type>();
        public static List<string> layoutNameList = new List<string>();
        public static GameObject layoutInstance;

        public static GameObject glassArtifact = Resources.Load<GameObject>("prefabs/pickupmodels/artifacts/PickupGlass");
        public static GameObject cubeObject = glassArtifact.transform.Find("mdlArtifactSimpleCube").gameObject;

        public static GameObject MeshPropsRef;
        public static Transform UI_OriginRef;

        public void Awake()
        {
            ConfigSetup.Bind(Config);
            ConfigSetup.InLobbyBind();
            CommandHelper.AddToConsoleWhenReady();
            AssemblySetup();

            On.RoR2.UI.CharacterSelectController.Awake += CharacterSelectController_Awake;
            // Hook Start instead?

        }
        

        private void CharacterSelectController_Awake(On.RoR2.UI.CharacterSelectController.orig_Awake orig, RoR2.UI.CharacterSelectController self)
        {
            orig(self);
            MeshPropsRef = GameObject.Find("MeshProps");
            UI_OriginRef = GameObject.Find("CharacterSelectUI").transform;


            // UI //
            Hook_ShowFade(UI_ShowFade.Value);
            Hook_BlurOpacity(UI_BlurOpacity.Value);
            Hook_UIScale(UI_Scale.Value);

            // Overlay //
            // Post Processing //
            Hook_ShowPostProcessing(PostProcessing.Value);
            Hook_Parallax(Parallax.Value);

            // Lights //
            Hook_LightUpdate_Color(Light_Color.Value);
            Hook_LightUpdate_Flicker(Light_Flicker.Value);
            Hook_LightUpdate_Intensity(Light_Intensity.Value);

            // Character Pad Displays //
            Hook_RescalePads(CharacterPadScale.Value);

            // Background Elements //
            Hook_HideProps(MeshProps.Value);
            Hook_HidePhysicsProps(PhysicsProps.Value);
            Hook_DisableShaking(Shaking.Value);

            Methods.LoadSceneAndLayout(SelectedScene.Value, SIL_SelectedLayout.Value);
        }


        public void AssemblySetup() //credit to bubbet for base code
        {
            var sceneType = typeof(LAIScene);
            var layoutType = typeof(CharSceneLayout);
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!type.IsAbstract)
                {
                    if (sceneType.IsAssignableFrom(type))
                    {
                        scenesDict[type.Name] = type;
                        sceneNameList.Add(type.Name);
                    }
                    else if (layoutType.IsAssignableFrom(type))
                    {
                        var sceneObjectInitializer = (CharSceneLayout)Activator.CreateInstance(type);
                        bool canLoadScene = true;
                        var guids = sceneObjectInitializer.RequiredModGUID;
                        if (guids != null && guids.Length > 0)
                        {
                            foreach (var GUID in guids) //Todo: Add optional assembly: "a.b.c||a.b.d"
                            {
                                //if (printpala) Debug.Log("current GUID: "+GUID);
                                if (!BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(GUID))
                                {
                                    canLoadScene = false;
                                    break;
                                }
                            }
                        }
                        if (canLoadScene)
                        {
                            layoutsDict[type.Name] = type;
                            layoutNameList.Add(type.Name);
                            //var selectedLayout = layoutsDict.TryGetValue(type.Name, out var layout);
                            Debug.Log("Initializing " + type);
                            sceneObjectInitializer.Init();
                        }
                    }
                }
            }
        }
    }
}