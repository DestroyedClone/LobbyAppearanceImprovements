using LobbyAppearanceImprovements.CharacterSceneLayouts;
using LobbyAppearanceImprovements.Scenes;
using RoR2;
using System;
using UnityEngine;
using static LobbyAppearanceImprovements.ConfigSetup;
using static LobbyAppearanceImprovements.LAIPlugin;
using System.Linq;
using System.Collections;
using UnityEngine.EventSystems;
using RoR2.UI;
using RoR2.UI.SkinControllers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using static UnityEngine.ColorUtility;
using LeTai.Asset.TranslucentImage;

//using static LobbyAppearanceImprovements.StaticValues;

namespace LobbyAppearanceImprovements
{
    public static class Methods
    {
        public static void ChangeLobbyLightColor(Color32 color)
        {
            GameObject.Find("Directional Light").gameObject.GetComponent<Light>().color = color;
        }

        public static GameObject CreateDisplay(string bodyPrefabName, Vector3 position, Vector3 rotation, Transform parent = null, bool addCollider = false)
        {
            Debug.Log("Attempting to display "+bodyPrefabName);
            var bodyPrefab = GetBodyPrefab(bodyPrefabName);
            if (!bodyPrefab)
            {
                Debug.LogWarning("Aborted attempting to load body prefab "+bodyPrefabName);
                return null;
            }

            SurvivorDef survivorDef = SurvivorCatalog.FindSurvivorDefFromBody(bodyPrefab);
            if (!survivorDef)
            {
                Debug.LogWarning("survivorDef missing");
                return null;
            }
            GameObject displayPrefab = survivorDef.displayPrefab;
            var gameObject = UnityEngine.Object.Instantiate<GameObject>(displayPrefab, position, Quaternion.Euler(rotation), parent);
            if (addCollider && ConfigSetup.SIL_ClickOnCharacterToSwap.Value)
            {
                var comp = gameObject.AddComponent<CapsuleCollider>();
                comp.radius = 1f;
                var com = gameObject.AddComponent<ClickToSelectCharacter>();
                com.survivorDef = survivorDef;
            }
            if (SIL_LockedCharactersBlack.Value)
            {
                var hasUnlocked = LocalUserManager.GetFirstLocalUser().userProfile.HasUnlockable(survivorDef.unlockableDef);
                if (!hasUnlocked)
                {
                    var cm = gameObject.transform.GetComponentsInChildren<CharacterModel>();
                    if (cm.Length > 0)
                    {
                        cm[0].isDoppelganger = true;
                    }
                }
            }
            switch (bodyPrefabName)
            {
                case "Croco":
                    gameObject.transform.Find("mdlCroco")?.transform.Find("Spawn")?.transform.Find("FloorMesh")?.gameObject.SetActive(false);
                    break;

                case "RobEnforcer":
                    break;
                case "Treebot":
                    gameObject.transform.Find("ModelBase/mdlTreebot").gameObject.GetComponent<CharacterModel>().enabled = false;
                    break;
                case "Toolbot":
                    gameObject.transform.Find("Base/mdlToolbot").gameObject.GetComponent<CharacterModel>().enabled = false;
                    break;
                case "HANDOverclocked":
                    GameObject.Find("HANDTeaser")?.SetActive(false);
                    break;
                case "RobPaladin":
                    if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rob.Paladin"))
                    {
                        SetupPaladinDisplay(gameObject);
                    }
                    break;
            }
            return gameObject;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void SetupPaladinDisplay(GameObject gameObject)
        {
            UnityEngine.Object.Destroy(gameObject.GetComponent<PaladinMod.Misc.MenuSound>());
        }


        public static GameObject GetBodyPrefab(string bodyPrefabName)
        {
            switch (bodyPrefabName)
            {
                case "CHEF":
                    break;

                default:
                    bodyPrefabName += "Body";
                    break;
            }
            var bodyPrefab = BodyCatalog.FindBodyPrefab(bodyPrefabName);
            if (!bodyPrefab) return null;
            return bodyPrefab;
        }

        public static void SelectScene(LAIScene scene)
        {
            if (sceneInstance)
                UnityEngine.Object.Destroy(sceneInstance);

            var sceneObject = (LAIScene)Activator.CreateInstance(scenesDict[SelectedScene.Value]);
            chosenScene = sceneObject;
            sceneInstance = sceneObject.CreateScene();
        }

        public static void SelectScene(string sceneName)
        {
            var selectedScene = scenesDict.TryGetValue(sceneName, out var scene);
            if (!selectedScene)
            {
                Debug.LogError("Requested Scene " + sceneName + " returned null!");
                return;
            }

            if (sceneInstance)
            {
                chosenScene.OnDestroy();
                UnityEngine.Object.Destroy(sceneInstance);
            }

            var sceneObject = (LAIScene)Activator.CreateInstance(scene);
            chosenScene = sceneObject;
            sceneInstance = sceneObject.CreateScene();
        }

        public static void SelectLayout(string layoutName)
        {
            var selectedLayout = layoutsDict.TryGetValue(layoutName, out var layout);
            if (!selectedLayout)
            {
                Debug.LogError("Requested Layout " + layoutName + " returned null!");
                return;
            }

            if (layoutInstance)
                UnityEngine.Object.Destroy(layoutInstance);

            var layoutObject = (CharSceneLayout)Activator.CreateInstance(layout);
            chosenLayout = layoutObject;
            layoutInstance = layoutObject.CreateLayout();
        }

        public static string GetDefaultLayoutNameForScene(string sceneName)
        {
            foreach (var kvp in layoutsDict)
            {
                if (kvp.Key.ToLower().Contains(sceneName.ToLower()) && kvp.Key.ToLower().Contains("default"))
                    return kvp.Key;
            }
            return null;
        }
        public static void LoadSceneAndLayout(string sceneName, string layoutName = null)
        {
            var currentSceneIsNotLobby = sceneName != (string)SelectedScene.DefaultValue;
            var sceneNameForLayout = currentSceneIsNotLobby ? sceneName : "lobby";

            if (currentSceneIsNotLobby)
            {
                if (!scenesDict.ContainsKey(sceneName))
                {
                    Debug.Log("Aborting: Selected Scene Not Found '" + sceneName + "'");
                    return;
                }
                MeshPropsRef.SetActive(false);
                Methods.SelectScene(sceneName);
            }
            else
            {
                MeshPropsRef.SetActive(true);
            }
            if (SIL_Enabled.Value)
                if (layoutName != (string)SIL_SelectedLayout.DefaultValue)
                {
                    Methods.SelectLayout(layoutName);
                }
                else
                {
                    var defaultLayoutName = Methods.GetDefaultLayoutNameForScene(sceneNameForLayout);
                    if (defaultLayoutName != null)
                    {
                        Methods.SelectLayout(defaultLayoutName);
                    }
                }
        }


        public class ClickToSelectCharacter : MonoBehaviour
        {
            //CapsuleCollider capsuleCollider;
            BoxCollider boxCollider;
            public SurvivorDef survivorDef;
            public Highlight highlight;
            public bool survivorUnlocked = false;
            public LocalUser localUser;
            CharacterSelectController characterSelectController;
            bool screenIsFocused = true;

            public void Start()
            {
                characterSelectController = GameObject.Find("CharacterSelectUI").GetComponent<CharacterSelectController>();
                localUser = ((MPEventSystem)EventSystem.current).localUser;
                if (survivorDef)
                {
                    survivorUnlocked = SurvivorCatalog.SurvivorIsUnlockedOnThisClient(survivorDef.survivorIndex);

                    highlight = gameObject.AddComponent<Highlight>();
                    highlight.highlightColor = survivorUnlocked ? Highlight.HighlightColor.interactive : Highlight.HighlightColor.unavailable;
                    highlight.isOn = false;
                    highlight.targetRenderer = GetTargetRenderer(survivorDef.cachedName);
                }
                else
                    Debug.LogWarning("No survivorDef found for " + gameObject.name);

                /*capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
                    capsuleCollider.contactOffset = 0.1f;
                    capsuleCollider.radius = 0.5f;
                    capsuleCollider.height = 1.82f;
                    capsuleCollider.direction = 1;
                capsuleCollider.center = Vector3.up* 1.2f;*/

                //SetupBoxCollider();
                SetupBoxColliderOld();
            }

            public void SetupBoxCollider()
            {
                boxCollider = gameObject.AddComponent<BoxCollider>();

                var renderer = highlight.targetRenderer;

                var bounds = renderer.bounds;
                // In world-space!
                var size = bounds.size;
                var center = bounds.center;

                // converted to local space of the collider
                //size = boxCollider.transform.InverseTransformVector(size);
                //center = boxCollider.transform.InverseTransformPoint(center);

                boxCollider.size = size;
                boxCollider.center = center;
            }

            //https://stackoverflow.com/questions/62986775/how-can-get-the-size-of-an-object-and-add-boxcollider-that-will-cover-automatic
            public void SetupBoxColliderOld()
            {
                boxCollider = gameObject.AddComponent<BoxCollider>();

                var renderer = highlight.targetRenderer;

                var bounds = renderer.bounds;
                // In world-space!
                var size = bounds.size;
                var center = bounds.center;

                // converted to local space of the collider
                size = boxCollider.transform.InverseTransformVector(size);
                center = boxCollider.transform.InverseTransformPoint(center);

                boxCollider.size = size;
                boxCollider.center = center;

            }

            public SkinnedMeshRenderer GetTargetRenderer(string cachedName)
            {
                Debug.Log("Checking cached name " + cachedName);
                string path = "";

                switch (cachedName)
                {
                    case "Commando":
                        path = "mdlCommandoDualies/CommandoMesh";
                        break;
                    case "Huntress":
                        path = "mdlHuntress (1)/HuntressMesh";
                        break;
                    case "Engi":
                    case "Mage":
                    case "Merc":
                    case "Croco":
                        path = "mdl"+ cachedName + "/"+ cachedName + "Mesh";
                        break;
                    case "Toolbot":
                        path = "Base/mdlToolbot/ToolbotMesh";
                        break;
                    case "Treebot":
                        path = "";
                        break;
                    case "Loader":
                        break;
                    case "Enforcer":
                        path = "meshEnforcer";
                        break;
                    case "Nemforcer":
                        path = "Nemforcer";
                        break;
                    case "SniperClassic":
                        path = "SniperMesh";
                        break;
                    case "HAND":
                    case "HAN-D":
                        path = "HAN-DMesh";
                        break;
                    case "Miner":
                        path = "MinerDisplay/MinerBody";
                        break;
                    case "RobPaladin":
                        path = "meshPaladin";
                        break;
                    case "CHEF":
                    case "Chef":
                        path = "Chef";
                        break;
                    default:
                        break;
                }
                Transform transform = gameObject.transform.Find(path);
                return path != "" ? transform?.GetComponent<SkinnedMeshRenderer>() : null;
            }

            public SkinnedMeshRenderer GetTargetRendererFallback()
            {
                var cow = GetComponentsInChildren<Transform>();
                var displayNameLower = Language.GetString(survivorDef.displayNameToken).ToLower();
                foreach (Transform calf in cow)
                {
                    var shark = calf.name.ToLower();
                    if (shark.Contains(displayNameLower) && shark.Contains("mesh"))
                    {
                        Debug.Log("4");
                        var comp = calf.gameObject.GetComponent<SkinnedMeshRenderer>();
                        if (comp) return comp;
                    }
                }
                return null;
            }

            public void OnMouseOver()
            {
                if (screenIsFocused && Input.GetKey(KeyCode.Mouse0))
                {
                    if (!survivorUnlocked)
                        return;
                    characterSelectController.SelectSurvivor(survivorDef.survivorIndex);
                    characterSelectController.SetSurvivorInfoPanelActive(true);
                    localUser.currentNetworkUser?.CallCmdSetBodyPreference(BodyCatalog.FindBodyIndex(survivorDef.bodyPrefab));
                    return;
                }
            }
            public void OnApplicationFocus(bool hasFocus)
            {
                screenIsFocused = hasFocus;
            }

            public void OnMouseEnter()
            {
                if (highlight) highlight.isOn = true;
            }
            public void OnMouseExit()
            {
                if (highlight) highlight.isOn = false;
            }
        }
        public class CameraParallax : MonoBehaviour
        {
            public GameObject sceneCamera;
            public Vector3 startingPosition;

            public Vector3 desiredPosition;
            private Vector3 velocity;
            public float screenLimitDistance = 0.25f;
            public float forwardLimit = 5f;
            public float forwardMult = 0.25f;

            bool screenIsFocused = true;

            public void Awake()
            {
                sceneCamera = GameObject.Find("Main Camera/Scene Camera");
                startingPosition = sceneCamera.transform.position;
                desiredPosition = startingPosition;
            }

            public void Update()
            {
                if (screenIsFocused)
                {
                    desiredPosition = dicks();
                }

                DampPosition();
            }
            void OnApplicationFocus(bool hasFocus)
            {
                screenIsFocused = hasFocus;
            }

            public void DampPosition()
            {
                sceneCamera.transform.position = Vector3.SmoothDamp(sceneCamera.transform.position, desiredPosition, ref velocity, 0.4f, float.PositiveInfinity, Time.deltaTime);
            }

            public Vector3 dicks()
            {
                Vector3 mousePos = Input.mousePosition;
                var value = new Vector3();

                float fractionX = (Screen.width - mousePos.x) / Screen.width;
                float fractionY = (Screen.height - mousePos.y) / Screen.height;

                value.x = Mathf.Lerp(startingPosition.x + screenLimitDistance, startingPosition.x - screenLimitDistance, fractionX);
                value.y = Mathf.Lerp(startingPosition.y + screenLimitDistance, startingPosition.y - screenLimitDistance, fractionY);

                //if (Input.GetMouseButtonDown(2))
                    //value.z = startingPosition.z;

                var val = startingPosition.z + Input.mouseScrollDelta.y * forwardMult;
                value.z = Mathf.Clamp(val, startingPosition.z - forwardLimit, startingPosition.z + forwardLimit);
                return value;
            }

            public void OnDisable()
            {
                desiredPosition = startingPosition;
            }
        }

        public class DelaySetupMeshCollider : MonoBehaviour
        {
            public float delayInSeconds;
            public MeshCollider meshCollider;
            public Mesh meshToBind;
            float stopwatch = 0f;

            public void Update()
            {
                stopwatch += Time.deltaTime;
                if (stopwatch >= delayInSeconds)
                {
                    meshCollider.sharedMesh = meshToBind;
                    enabled = false;
                }
            }
        }
    }

    public static class SceneMethods
    {
        public static List<string> GetScenes()
        {
            Debug.Log(sceneNameList.Count);
            return sceneNameList;
        }
    }

    public static class HookMethods
    {
        public static void Hook_ShowFade(bool value)
        {
            UI_ShowFade.Value = value;
            UI_OriginRef.Find("BottomSideFade").gameObject.SetActive(value);
            UI_OriginRef.Find("TopSideFade").gameObject.SetActive(value);
        }

        public static void Hook_DisableShaking(bool value)
        {
            Shaking.Value = value;
            var shaker = UnityEngine.Object.FindObjectOfType<PreGameShakeController>();
            if (shaker)
                shaker.enabled = value;
        }

        public static void Hook_BlurOpacity(int value)
        {
            ConfigSetup.UI_BlurOpacity.Value = (int)Mathf.Clamp(value, 0f, 255);

            var SafeArea = UI_OriginRef.Find("SafeArea").transform;
            var ui_left = SafeArea.Find("LeftHandPanel (Layer: Main)");
            var ui_right = SafeArea.Find("RightHandPanel");

            var leftBlurColor = ui_left.Find("BlurPanel").GetComponent<TranslucentImage>();
            leftBlurColor.color = new Color(leftBlurColor.color.r,
                leftBlurColor.color.g,
                leftBlurColor.color.b,
                UI_BlurOpacity.Value);
            var rightBlurColor = ui_right.Find("RuleVerticalLayout").Find("BlurPanel").GetComponent<TranslucentImage>();
            rightBlurColor.color = new Color(leftBlurColor.color.r,
                rightBlurColor.color.g,
                rightBlurColor.color.b,
                UI_BlurOpacity.Value);
        }

        public static void Hook_UIScale(float value)
        {
            var SafeArea = UI_OriginRef.Find("SafeArea").transform;
            var ui_left = SafeArea.Find("LeftHandPanel (Layer: Main)");
            var ui_right = SafeArea.Find("RightHandPanel");

            ui_left.localScale = Vector3.one * value;
            ui_right.localScale = Vector3.one * value;
        }
        public static void Hook_ShowPostProcessing(bool value)
        {
            PostProcessing.Value = value;
            GameObject.Find("PP")?.SetActive(value);
        }

        public static void Hook_LightUpdate_Color(string color)
        {
            Light_Color.Value = color;
            var directionalLight = GameObject.Find("Directional Light");

            if (directionalLight)
            {
                if ((Light_Color.Value != "default" || Light_Color.Value != null) && TryParseHtmlString(Light_Color.Value, out Color newColor))
                    Methods.ChangeLobbyLightColor(newColor);
            }
        }

        public static void Hook_LightUpdate_Flicker(bool flicker)
        {
            Light_Flicker.Value = flicker;
            var directionalLight = GameObject.Find("Directional Light");
            if (directionalLight)
            {
                directionalLight.gameObject.GetComponent<FlickerLight>().enabled = Light_Flicker.Value;
            }
        }
        public static void Hook_LightUpdate_Intensity(float intensity)
        {
            Light_Intensity.Value = intensity;
            var directionalLight = GameObject.Find("Directional Light");
            if (directionalLight)
            {
                directionalLight.gameObject.GetComponent<Light>().intensity = Light_Intensity.Value;
            }
        }

        public static void Hook_RescalePads(float size)
        {
            CharacterPadScale.Value = size;
            var characterPadAlignments = GameObject.Find("CharacterPadAlignments");

            if (characterPadAlignments)
            {
                //if (LobbyViewType != StaticValues.LobbyViewType.Zoom) //if Zoom is selected, then this will NRE //here
                characterPadAlignments.transform.localScale *= CharacterPadScale.Value;

            }
        }

        public static void Hook_HideProps(bool value)
        {
            MeshProps.Value = value;

            foreach (var propName in MeshPropNames)
            {
                GameObject.Find(propName)?.SetActive(MeshProps.Value);
            }
            Hook_HidePhysicsProps(PhysicsProps.Value);
        }

        public static void Hook_HidePhysicsProps(bool value)
        {
            PhysicsProps.Value = value;

            var meshPropHolder = MeshPropsRef.transform;
            if (meshPropHolder)
            {
                foreach (var propName in PhysicsPropNames)
                {
                    meshPropHolder.Find(propName)?.gameObject.SetActive(value);
                }
            }
        }

        public static void Hook_Parallax(bool value)
        {
            Parallax.Value = value;

            var csc = UnityEngine.Object.FindObjectOfType<RoR2.UI.CharacterSelectController>();
            if (csc)
            {
                var para = csc.GetComponent<Methods.CameraParallax>();
                if (!para)
                {
                    para = csc.gameObject.AddComponent<Methods.CameraParallax>();
                }
                para.enabled = value;
            }
        }
    }
}