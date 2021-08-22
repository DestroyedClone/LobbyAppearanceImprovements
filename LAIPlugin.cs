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

namespace LobbyAppearanceImprovements
{
    [BepInDependency(R2API.R2API.PluginGUID, R2API.R2API.PluginVersion)]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    [BepInPlugin(ModGuid, ModName, ModVer)]
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

        public static List<SurvivorDef> mousedOverObjects = new List<SurvivorDef>();

        public static GameObject PickupEliteOnlyPrefab = Resources.Load<GameObject>("prefabs/pickupmodels/artifacts/PickupEliteOnly");

        public void Awake()
        {
            ConfigSetup.Bind(Config);
            CommandHelper.AddToConsoleWhenReady();
            AssemblySetup();

            On.RoR2.UI.CharacterSelectController.Awake += CharacterSelectController_Awake;
            // Hook Start instead?

            if (DisableShaking.Value)
                On.RoR2.PreGameShakeController.Awake += SetShakerInactive;
        }

        private void CharacterSelectController_Awake(On.RoR2.UI.CharacterSelectController.orig_Awake orig, RoR2.UI.CharacterSelectController self)
        {
            orig(self);

            self.gameObject.AddComponent<Methods.ClickToSetFirstEntryAsChar>().characterSelectController = self;

            var directionalLight = GameObject.Find("Directional Light");
            var ui_origin = GameObject.Find("CharacterSelectUI").transform;
            var SafeArea = ui_origin.Find("SafeArea").transform;
            var ui_left = SafeArea.Find("LeftHandPanel (Layer: Main)");
            var ui_right = SafeArea.Find("RightHandPanel");
            var characterPadAlignments = GameObject.Find("CharacterPadAlignments");

            // UI //
            if (ui_origin)
            {
                if (HideFade.Value)
                {
                    ui_origin.Find("BottomSideFade").gameObject.SetActive(false);
                    ui_origin.Find("TopSideFade").gameObject.SetActive(false);
                }
                if (BlurValue.Value != 255) // default value doesnt cast well
                {
                    var leftBlurColor = ui_left.Find("BlurPanel").GetComponent<TranslucentImage>().color;
                    leftBlurColor.a = Mathf.Clamp(BlurValue.Value, 0f, 255f);
                    var rightBlurColor = ui_right.Find("RuleVerticalLayout").Find("BlurPanel").GetComponent<TranslucentImage>().color;
                    rightBlurColor.a = Mathf.Clamp(BlurValue.Value, 0f, 255f);
                }
                if (UIScale.Value != 1f)
                {
                    ui_left.localScale *= UIScale.Value;
                    ui_right.localScale *= UIScale.Value;
                }
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
                var meshPropHolder = GameObject.Find("MeshProps").transform;
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

            RestartSceneAndLayout(SelectedScene.Value, SelectedLayout.Value);
        }

        public void RestartSceneAndLayout(string sceneName, string layoutName = null)
        {
            var currentSceneIsNotLobby = sceneName != (string)SelectedScene.DefaultValue;

            if (currentSceneIsNotLobby)
            {
                if (!scenesDict.ContainsKey(sceneName))
                {
                    Debug.Log("Selected Scene Not Found : " + sceneName);
                    return;
                }
                GameObject.Find("MeshProps")?.SetActive(false);
                Methods.SelectScene(chosenScene);

                if (SurvivorsInLobby.Value)
                    if (layoutName != (string)SelectedLayout.DefaultValue)
                    {
                        Methods.SelectLayout(SelectedLayout.Value);
                    }
                    else
                    {
                        var defaultLayoutName = Methods.GetDefaultLayoutNameForScene(sceneName);
                        if (defaultLayoutName != null)
                        {
                            Methods.SelectLayout(defaultLayoutName);
                        }
                    }
            }
            else
            {
                GameObject.Find("MeshProps")?.SetActive(true);
                if (SurvivorsInLobby.Value)
                    if (layoutName != (string)SelectedLayout.DefaultValue)
                    {
                        Methods.SelectLayout(SelectedLayout.Value);
                    }
                    else
                    {
                        var defaultLayoutName = Methods.GetDefaultLayoutNameForScene("lobby");
                        if (defaultLayoutName != null)
                        {
                            Methods.SelectLayout(defaultLayoutName);
                        }
                    }
            }
        }

        public void AssemblySetup() //credit to bubbet for base code
        {
            var sceneType = typeof(LAIScene);
            var layoutType = typeof(CharacterSceneLayouts.CharSceneLayout);
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (sceneType.IsAssignableFrom(type))
                {
                    scenesDict[type.Name] = type;
                }
                else if (layoutType.IsAssignableFrom(type))
                {
                    layoutsDict[type.Name] = type;
                }
            }
            if (SelectedScene.Value.ToLower() != "default" && scenesDict[SelectedScene.Value] != null)
            {
                var sceneObject = (LAIScene)Activator.CreateInstance(scenesDict[SelectedScene.Value]);
                chosenScene = sceneObject;
            }
            if (SelectedLayout.Value.ToLower() != "default" && layoutsDict[SelectedLayout.Value] != null)
            {
                var layoutObject = (CharSceneLayout)Activator.CreateInstance(layoutsDict[SelectedLayout.Value]);
                chosenLayout = layoutObject;
            }
        }

        private void SetShakerInactive(On.RoR2.PreGameShakeController.orig_Awake orig, PreGameShakeController self)
        {
            orig(self);
            self.gameObject.SetActive(false);
        }
    }
}