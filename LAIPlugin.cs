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
        public static GameObject sceneInstance;

        public static CharSceneLayout chosenLayout = null;
        public static Dictionary<string, Type> layoutsDict = new Dictionary<string, Type>();
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

            if (DisableShaking.Value)
                On.RoR2.PreGameShakeController.Awake += SetShakerInactive;

        }
        public static void Hook_ShowFade(bool value)
        {
            UI_OriginRef.Find("BottomSideFade").gameObject.SetActive(value);
            UI_OriginRef.Find("TopSideFade").gameObject.SetActive(value);
        }

        public static void Hook_BlurOpacity(int value)
        {
            var SafeArea = UI_OriginRef.Find("SafeArea").transform;
            var ui_left = SafeArea.Find("LeftHandPanel (Layer: Main)");
            var ui_right = SafeArea.Find("RightHandPanel");

            var leftBlurColor = ui_left.Find("BlurPanel").GetComponent<TranslucentImage>();
            leftBlurColor.color = new Color(leftBlurColor.color.r,
                leftBlurColor.color.g,
                leftBlurColor.color.b,
                Mathf.Clamp(value, 0f, 255));
            var rightBlurColor = ui_right.Find("RuleVerticalLayout").Find("BlurPanel").GetComponent<TranslucentImage>();
            rightBlurColor.color = new Color(leftBlurColor.color.r,
                rightBlurColor.color.g,
                rightBlurColor.color.b,
                Mathf.Clamp(value, 0f, 255));
        }

        public static void Hook_UIScale(float value)
        {
            var SafeArea = UI_OriginRef.Find("SafeArea").transform;
            var ui_left = SafeArea.Find("LeftHandPanel (Layer: Main)");
            var ui_right = SafeArea.Find("RightHandPanel");

            ui_left.localScale = Vector3.one * value;
            ui_right.localScale = Vector3.one * value;
        }

        private void CharacterSelectController_Awake(On.RoR2.UI.CharacterSelectController.orig_Awake orig, RoR2.UI.CharacterSelectController self)
        {
            orig(self);
            MeshPropsRef = GameObject.Find("MeshProps");

            self.gameObject.AddComponent<Methods.CameraParallax>();

            var directionalLight = GameObject.Find("Directional Light");
            UI_OriginRef = GameObject.Find("CharacterSelectUI").transform;
            var SafeArea = UI_OriginRef.Find("SafeArea").transform;
            var ui_left = SafeArea.Find("LeftHandPanel (Layer: Main)");
            var ui_right = SafeArea.Find("RightHandPanel");
            var characterPadAlignments = GameObject.Find("CharacterPadAlignments");

            // UI //
            if (UI_OriginRef)
            {
                Hook_ShowFade(UI_ShowFade.Value);
                Hook_BlurOpacity(UI_BlurOpacity.Value);
                Hook_UIScale(UI_Scale.Value);
            }

            // Overlay //
            // Post Processing //
            if (PostProcessing.Value)
            {
                GameObject.Find("PP")?.SetActive(false);
            }

            // Lights //
            if (directionalLight)
            {
                if (Light_Color.Value != "default" && TryParseHtmlString(Light_Color.Value, out Color color))
                    Methods.ChangeLobbyLightColor(color);
                directionalLight.gameObject.GetComponent<Light>().intensity = Light_Intensity.Value;
                directionalLight.gameObject.GetComponent<FlickerLight>().enabled = !Light_Flicker_Disable.Value;
            }

            // Character Pad Displays //
            if (characterPadAlignments)
            {
                if (CharacterPadScale.Value != 1f)
                {
                    //if (LobbyViewType != StaticValues.LobbyViewType.Zoom) //if Zoom is selected, then this will NRE //here
                    characterPadAlignments.transform.localScale *= CharacterPadScale.Value;
                }
            }

            // Background Elements //
            if (MeshProps.Value)
            {
                foreach (var propName in MeshPropNames)
                {
                    GameObject.Find(propName)?.SetActive(false);
                }
            }
            if (PhysicsProps.Value)
            {
                var meshPropHolder = MeshPropsRef.transform;
                if (meshPropHolder)
                {
                    if (MeshProps.Value)
                    {
                        // MeshProps holds both static and physics, so we save processing time(?) if we just disable the whole thing.
                        meshPropHolder.gameObject.SetActive(false);
                    }
                    else
                    {
                        foreach (var propName in PhysicsPropNames)
                        {
                            meshPropHolder.Find(propName)?.gameObject.SetActive(false);
                        }
                    }
                }
            }

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
                            //var selectedLayout = layoutsDict.TryGetValue(type.Name, out var layout);
                            Debug.Log("Initializing " + type);
                            sceneObjectInitializer.Init();
                        }
                    }
                }
            }
        }

        private void SetShakerInactive(On.RoR2.PreGameShakeController.orig_Awake orig, PreGameShakeController self)
        {
            orig(self);
            self.gameObject.SetActive(false);
        }
    }
}