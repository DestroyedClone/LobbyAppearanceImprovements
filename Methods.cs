﻿using BepInEx;
using LeTai.Asset.TranslucentImage;
using LobbyAppearanceImprovements.CharacterSceneLayouts;
using LobbyAppearanceImprovements.Scenes;
using RoR2;
using RoR2.SurvivorMannequins;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using static LobbyAppearanceImprovements.ConfigSetup;
using static LobbyAppearanceImprovements.LAIPlugin;
using static UnityEngine.ColorUtility;

//using static LobbyAppearanceImprovements.StaticValues;

namespace LobbyAppearanceImprovements
{
    public static class Methods
    {
        public static void ChangeLobbyLightColor(Color32 color)
        {
            GameObject.Find("Directional Light").GetComponent<Light>().color = color;
        }

        public class LAI_CharDisplayTracker : MonoBehaviour
        {
            public CharacterModel characterModel;
            public bool hasUnlocked = false;

            public void Awake()
            {
                InstanceTracker.Add(this);
            }

            public void OnDestroy()
            {
                InstanceTracker.Remove(this);
            }

            public void ToggleShadow(bool value)
            {
                if (characterModel)
                    characterModel.isDoppelganger = value && !hasUnlocked;
            }
        }

        public static void SetCamera(CameraRigController cameraRig, CharSceneLayout.CameraSetting cameraSetting)
        {
            SetCamera(cameraRig, cameraSetting.fov, cameraSetting.position, cameraSetting.rotation);
            //add pos and rot
        }

        public static void SetCamera(CameraRigController cameraRig = null, float fov = 60f, Vector3 position = default, Vector3 rotation = default)
        {
            if (!cameraRig)
            {
                cameraRig = LAICameraManager.MainCameraRigController;
            }

            var modifier = 0f;
            if (fov != 60f) //todo add pos and rot //used to 'bounce' the camera when selecting different characters
            {
                float threshold = 5f;
                float min = fov - threshold;
                float max = fov + threshold;
                var modifierthreshold = 3f;
                if (min <= cameraRig.baseFov && cameraRig.baseFov <= max)
                {
                    modifier = UnityEngine.Random.Range(-modifierthreshold, modifierthreshold);
                }
            }
            cameraRig.baseFov = fov + modifier;
            var currentCam = LAICameraManager.CurrentCameraController;
            var desiredPosition = position == default ? currentCam.DefaultPosition : position;
            currentCam.desiredCenterPosition = desiredPosition;
            var desiredRotation = rotation == default ? currentCam.DefaultRotation : Quaternion.Euler(rotation);
            currentCam.desiredRotation = desiredRotation;

            /*cameraRig.GenerateCameraModeContext(out RoR2.CameraModes.CameraModeBase.CameraModeContext cameraModeContext);
            //object rawInstanceData = cameraRig.cameraMode.camToRawInstanceData[cameraModeContext.cameraInfo.cameraRigController];
            object rawInstanceData = cameraRig.cameraMode.camToRawInstanceData[cameraRig];
            ((RoR2.CameraModes.CameraModePlayerBasic.InstanceData)rawInstanceData).pitchYaw = new PitchYawPair()
            {
                pitch = pitch,
                yaw = yaw
            };
            cameraRig.cameraModeContext = cameraModeContext;*/
            //cameraRig.pitch = pitch;
            //cameraRig.yaw = yaw;
        }

        public static GameObject CreateDisplay(string bodyPrefabName, Vector3 position, Vector3 rotation, Transform parent = null, bool addCollider = false)
        {
            //Debug.Log("Attempting to display "+bodyPrefabName);
            bool strictName = false;
            if (bodyPrefabName.StartsWith("!"))
            {
                strictName = true;
                bodyPrefabName = bodyPrefabName.Substring(1);
            }
            var bodyPrefab = GetBodyPrefab(bodyPrefabName, strictName);
            if (!bodyPrefab)
            {
                LAILogging.LogMessage("CreateDisplay :: Aborted, no body prefab found for " + bodyPrefabName, ConfigSetup.LoggingStyle.UserShouldSee);
                return null;
            }

            SurvivorDef survivorDef = SurvivorCatalog.FindSurvivorDefFromBody(bodyPrefab);
            if (!survivorDef)
            {
                LAILogging.LogMessage("CreateDisplay :: Aborted, no SurvivorDef found for " + bodyPrefabName, ConfigSetup.LoggingStyle.UserShouldSee);
                return null;
            }
            GameObject displayPrefab = survivorDef.displayPrefab;
            var gameObject = UnityEngine.Object.Instantiate<GameObject>(displayPrefab, position, Quaternion.Euler(rotation), parent);
            var trackerComponent = gameObject.AddComponent<LAI_CharDisplayTracker>();
            if (addCollider && ConfigSetup.SIL_ClickOnCharacterToSwap.Value)
            {
                var comp = gameObject.AddComponent<CapsuleCollider>();
                comp.radius = 1f;
                var com = gameObject.AddComponent<ClickToSelectCharacter>();
                com.survivorDef = survivorDef;
            }

            var hasUnlocked = LocalUserManager.GetFirstLocalUser().userProfile.HasUnlockable(survivorDef.unlockableDef);
            trackerComponent.hasUnlocked = hasUnlocked;
            var cm = gameObject.transform.GetComponentsInChildren<CharacterModel>();
            if (cm.Length > 0)
            {
                trackerComponent.characterModel = cm[0];
                trackerComponent.ToggleShadow(SIL_LockedCharactersBlack.Value);
                //cm[0].isDoppelganger = SIL_LockedCharactersBlack.Value;
            }

            switch (bodyPrefabName)
            {
                case "Croco":
                    gameObject.transform.Find("mdlCroco").transform.Find("Spawn").transform.Find("FloorMesh").gameObject.SetActive(false);
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
                    var teaser = LAISceneManager.sceneInstance.transform.Find("HANDTeaser");
                    if (teaser) teaser.gameObject.SetActive(false);
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

        public static GameObject GetBodyPrefab(string bodyPrefabName, bool strict = false)
        {
            if (!strict)
            {
                bodyPrefabName += "Body";
            }
            var bodyPrefab = BodyCatalog.FindBodyPrefab(bodyPrefabName);
            if (!bodyPrefab) return null;
            return bodyPrefab;
        }

        public static void SelectScene(string sceneName)
        {
            var sceneNameLower = sceneName.ToLower();
            var selectedScene = LAISceneManager.scenesDict.TryGetValue(sceneNameLower, out var scene);
            if (!selectedScene)
            {
                LAILogging.LogWarning($"SelectScene :: {sceneName} (parsed as \'{sceneNameLower})\' not found!", ConfigSetup.LoggingStyle.UserShouldSee);
                return;
            }

            if (LAISceneManager.sceneInstance)
            {
                LAISceneManager.chosenScene.OnDestroy();
                UnityEngine.Object.Destroy(LAISceneManager.sceneInstance);
            }

            //RefreshLobbyBackground
            if (PreGameController.instance && PreGameController.instance.lobbyBackground)
            {
                PreGameController.instance.lobbyBackground.SetActive(false);
            }

            var sceneObject = (LAIScene)Activator.CreateInstance(scene);
            LAISceneManager.chosenScene = sceneObject;
            sceneObject.CreateScene(true);
            ConfigSetup.Scene_Selection.Value = sceneNameLower;
        }

        public static void SelectLayout(string layoutName, bool saveChanges = true)
        {
            var layoutNameLower = layoutName.ToLower();
            var selectedLayout = LAILayoutManager.layoutsDict.TryGetValue(layoutNameLower, out var layout);
            if (!selectedLayout)
            {
                LAILogging.LogWarning($"SelectLayout :: {layoutName} \'(parsed as {layoutNameLower})\' not found!", ConfigSetup.LoggingStyle.UserShouldSee);
                return;
            }
            if (LAILayoutManager.layoutInstance)
            {
                if (ConfigSetup.SIL_SelectedLayout.Value.ToLower() == layoutNameLower)
                {
                    return;
                }
                LAILayoutManager.chosenLayout.OnDestroy();
                UnityEngine.Object.Destroy(LAILayoutManager.layoutInstance);
            }

            var layoutObject = (CharSceneLayout)Activator.CreateInstance(layout);
            LAILayoutManager.chosenLayout = layoutObject;
            LAILayoutManager.layoutInstance = layoutObject.CreateLayout();
            if (saveChanges)
                ConfigSetup.SIL_SelectedLayout.Value = layoutNameLower;

            //Resets camera on layout change
            SetCamera();
        }

        public static string GetDefaultLayoutNameForScene(string sceneName)
        {
            foreach (var kvp in LAILayoutManager.layoutsDict)
            {
                if (kvp.Key.ToLower().Contains(sceneName.ToLower()) && kvp.Key.ToLower().Contains("default"))
                    return kvp.Key;
                else
                    return nameof(Any_Empty);
            }
            return null;
        }

        public enum LoadSceneAndLayoutResult
        {
            NoSceneNoLayout,
            NoScene,
            NoLayout,
            Loaded
        };

        public static LoadSceneAndLayoutResult LoadSceneAndLayout(string sceneName, string layoutName = null, bool saveChanges = true)
        {
            bool resultScene = false;
            bool resultLayout = false;

            if (sceneName != null)
            {
                if (!LAISceneManager.scenesDict.ContainsKey(sceneName))
                {
                    LAILogging.LogWarning($"LoadSceneAndLayout :: Could not find scene \"{sceneName}\"!", ConfigSetup.LoggingStyle.UserShouldSee);
                }
                else
                {
                    Methods.SelectScene(sceneName);
                    resultScene = true;
                }
            }
            if (layoutName.IsNullOrWhiteSpace())
            {
                layoutName = LAISceneManager.chosenScene.PreferredLayout != null ? LAISceneManager.chosenScene.PreferredLayout : nameof(Any_Empty);
            }
            if (layoutName != (string)SIL_SelectedLayout.DefaultValue)
            {
                Methods.SelectLayout(layoutName, saveChanges);
                resultLayout = true;
            }
            return UnderstandConceptOfLove(resultScene, resultLayout);
        }

        //note to self: stop naming methods stupid names
        //the fuck is this for?
        public static LoadSceneAndLayoutResult UnderstandConceptOfLove(bool resultScene, bool resultLayout)
        {
            if (resultScene && resultLayout)
                return LoadSceneAndLayoutResult.Loaded;
            if (resultScene && !resultLayout)
                return LoadSceneAndLayoutResult.NoLayout;
            if (!resultScene && resultLayout)
                return LoadSceneAndLayoutResult.NoScene;
            return LoadSceneAndLayoutResult.NoSceneNoLayout;
        }

        public class ClickToSelectCharacter : MonoBehaviour
        {
            //CapsuleCollider capsuleCollider;
            private BoxCollider boxCollider;

            public SurvivorDef survivorDef;
            public Highlight highlight;
            public bool survivorUnlocked = false;
            public LocalUser localUser;
            private CharacterSelectController characterSelectController;
            private bool screenIsFocused = true;

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
                {
                    LAILogging.LogWarning("ClickToSelectCharacter :: No SurvivorDef found for " + gameObject.name, ConfigSetup.LoggingStyle.UserShouldSee);
                }

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
                LAILogging.LogMessage($"ClickToSelectCharacter.GetTargetRenderer :: Checking cached name {cachedName}.", ConfigSetup.LoggingStyle.UserShouldSee);
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
                        path = "mdl" + cachedName + "/" + cachedName + "Mesh";
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
                return path != "" && transform ? transform.GetComponent<SkinnedMeshRenderer>() : null;
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
                        //Debug.Log("4");
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
                    LocalUserManager.GetFirstLocalUser().userProfile.SetSurvivorPreference(survivorDef);
                    characterSelectController.SetSurvivorInfoPanelActive(true);
                    if (localUser.currentNetworkUser) localUser.currentNetworkUser.CallCmdSetBodyPreference(BodyCatalog.FindBodyIndex(survivorDef.bodyPrefab));
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

        public class LAICameraController : MonoBehaviour
        {
            public static LAICameraController instance;

            public GameObject sceneCamera;

            // Defaults
            public string sepDefaults = "==Defaults==";

            public Vector3 DefaultPosition { get; private set; }
            public Quaternion DefaultRotation { get; private set; }

            // Parallax
            public string setpParallax = "==Parallax==";

            public Vector3 desiredPosition;
            public Vector3 desiredCenterPosition;
            public Quaternion desiredRotation;
            private Vector3 dampPositionVelocity;
            private Quaternion dampRotationVelocity;
            private readonly float screenLimitDistance = 0.25f; //Limit of the screen to move with parallax from the center of the screen.
            private readonly float forwardLimit = 5f;
            private readonly float forwardMult = 0.25f;

            // Zoom On Character
            public string sepZoom = "==Zoom==";

            // Rotating Character
            public string sepRotate = "==Rotate==";

            public Vector3 rotate_initialPosition;
            public Vector3 rotate_currentPosition;
            public float rotate_multiplier = 2f;
            public Vector3 rotate_defaultRotationChar;

            // Other
            public string setpOther = "==Other==";

            private bool screenIsFocused = true;
            private Vector3 MousePosition;

            private bool mouse0Click = false;
            public CharacterSelectController characterSelectController;
            //public CharacterSelectController.CharacterPad[] characterPads = null;

            public RoR2.SurvivorMannequins.SurvivorMannequinSlotController[] survivorMannequinSlotControllers { get; set; }
            public SurvivorMannequinDioramaController survivorMannequinDioramaController = null;

            public void Awake()
            {
                if (!instance)
                {
                    instance = this;
                }
                else
                {
                    LAILogging.LogWarning("Two instances of LAICameraController were spawned?", ConfigSetup.LoggingStyle.UserShouldSee);
                }

                sceneCamera = GameObject.Find("Main Camera/Scene Camera");
                DefaultPosition = sceneCamera.transform.position;
                DefaultRotation = sceneCamera.transform.rotation;
                if (!characterSelectController)
                {
                    characterSelectController = UnityEngine.Object.FindObjectOfType<CharacterSelectController>();
                    survivorMannequinDioramaController = UnityEngine.Object.FindObjectOfType<SurvivorMannequinDioramaController>();
                    //characterPads = characterSelectController.characterDisplayPads;
                    survivorMannequinSlotControllers = survivorMannequinDioramaController.mannequinSlots;
                    //rotate_defaultRotationChar = characterPads[0].padTransform.eulerAngles;
                    //rotate_defaultRotationChar = survivorMannequinSlotControllers[0].mannequinInstanceTransform.eulerAngles;
                    rotate_defaultRotationChar = new Vector3(0f, 219.0844f, 0f);
                }

                desiredCenterPosition = DefaultPosition;
                desiredPosition = DefaultPosition;
                desiredRotation = DefaultRotation;

                if (LAICameraManager.CurrentCameraController != null && LAICameraManager.CurrentCameraController != this)
                {
                    LAILogging.LogWarning("Somehow there are two camera parallaxes?", ConfigSetup.LoggingStyle.UserShouldSee);
                }

                LAICameraManager.CurrentCameraController = this;
            }

            public void AdjustRotateSpeed(float speed)
            {
                rotate_multiplier = speed;
            }

            public void OnDestroy()
            {
                LAICameraManager.CurrentCameraController = null;
            }

            public void Update()
            {
                if (screenIsFocused)
                {
                    if (isFreeCam)
                    {
                        FreeCamPostion();
                    }
                    else
                    {
                        MousePosition = Input.mousePosition;

                        if (Parallax.Value)
                            desiredPosition = GetDesiredPositionFromScreenFraction();
                        if (MannequinEnableLocalTurn.Value)
                            RotateCamera();

                        if (Input.GetKeyDown(ConfigSetup.SIL_ResetCameraKey.Value))
                        {
                            Methods.SetCamera();
                        }
                    }
                }

                DampPosition();
                DampRotation();
            }

            //https://forum.unity.com/threads/quaternion-smoothdamp.793533/
            //https://gist.github.com/maxattack/4c7b4de00f5c1b95a33b
            private void DampRotation()
            {
                sceneCamera.transform.rotation = QuaternionUtil.SmoothDamp(sceneCamera.transform.rotation, desiredRotation, ref dampRotationVelocity, 0.4f);
            }

            public static bool isFreeCam = false;
            public float freeCamMultiplier = 1;

            public void FreeCamPostion()
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    desiredPosition += new Vector3(0f, 0f, freeCamMultiplier);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    desiredPosition += new Vector3(0f, 0f, -freeCamMultiplier);
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    desiredPosition += new Vector3(-freeCamMultiplier, 0f, 0f);
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    desiredPosition += new Vector3(freeCamMultiplier, 0f, 0f);
                }
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    desiredPosition += new Vector3(0f, freeCamMultiplier, 0f);
                }
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    desiredPosition += new Vector3(0f, -freeCamMultiplier, 0f);
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    desiredPosition = DefaultPosition;
                }

                if (Input.GetKeyDown(KeyCode.P))
                {
                    LAILogging.LogMessage($"position = new Vector3({desiredPosition.x}f, {desiredPosition.y}f, {desiredPosition.z}f)", LoggingStyle.None);
                }
            }

            public void RotateCamera(bool reset = false)
            {
                if (reset)
                {
                    rotate_initialPosition = DefaultPosition;
                    rotate_currentPosition = DefaultPosition;
                    //characterPads[0].padTransform.eulerAngles = rotate_defaultRotationChar;
                    if (survivorMannequinSlotControllers == null)
                    {
                        LAILogging.LogError($"survivorMannequinSlotControllers is missing!", ConfigSetup.LoggingStyle.UserShouldSee);
                        return;
                    }
                    if (survivorMannequinSlotControllers[0] == null)
                    {
                        LAILogging.LogError($"survivorMannequinSlotControllers[0] is missing!", ConfigSetup.LoggingStyle.UserShouldSee);
                        return;
                    }
                    if (survivorMannequinSlotControllers[0].mannequinInstanceTransform == null)
                    {
                        LAILogging.LogError($"survivorMannequinSlotControllers[0].mannequinInstanceTransform is missing!", ConfigSetup.LoggingStyle.UserShouldSee);
                        return;
                    }
                    survivorMannequinSlotControllers[0].mannequinInstanceTransform.eulerAngles = rotate_initialPosition;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    rotate_initialPosition = MousePosition;
                }
                if (Input.GetMouseButton(0))
                {
                    rotate_currentPosition = MousePosition;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    rotate_initialPosition = DefaultPosition;
                    rotate_currentPosition = DefaultPosition;
                    //characterPads[0].padTransform.eulerAngles = rotate_defaultRotationChar;
                    survivorMannequinSlotControllers[0].mannequinInstanceTransform.eulerAngles = rotate_defaultRotationChar;
                }

                if (survivorMannequinSlotControllers != null && survivorMannequinSlotControllers[0] != null && survivorMannequinSlotControllers[0].mannequinInstanceTransform)//characterPads != null)
                {
                    //var rotationVector = Vector3.Distance(rotate_initialPosition, rotate_currentPosition);
                    var rotationVector = rotate_initialPosition.x - rotate_currentPosition.x;
                    //var modifier = rotate_currentPosition.x < rotate_initialPosition.x ? 1 : -1;
                    var newRotation = rotationVector * rotate_multiplier;
                    //characterPads[0].padTransform.eulerAngles = rotate_defaultRotationChar + newRotation * Vector3.up;
                    survivorMannequinSlotControllers[0].mannequinInstanceTransform.eulerAngles = rotate_defaultRotationChar + newRotation * Vector3.up;
                }
            }

            private void OnApplicationFocus(bool hasFocus)
            {
                screenIsFocused = hasFocus;
            }

            public void DampPosition()
            {
                sceneCamera.transform.position = Vector3.SmoothDamp(sceneCamera.transform.position, desiredPosition, ref dampPositionVelocity, 0.4f, float.PositiveInfinity, Time.deltaTime);
            }

            public Vector3 GetDesiredPositionFromScreenFraction(bool reset = false)
            {
                if (reset)
                {
                    return Vector3.one * 0.5f;
                }
                var value = new Vector3();

                float fractionX = (Screen.width - MousePosition.x) / Screen.width;
                float fractionY = (Screen.height - MousePosition.y) / Screen.height;

                value.x = Mathf.Lerp(desiredCenterPosition.x + screenLimitDistance, desiredCenterPosition.x - screenLimitDistance, fractionX);
                value.y = Mathf.Lerp(desiredCenterPosition.y + screenLimitDistance, desiredCenterPosition.y - screenLimitDistance, fractionY);

                //if (Input.GetMouseButtonDown(2))
                //value.z = startingPosition.z;

                var val = desiredCenterPosition.z + Input.mouseScrollDelta.y * forwardMult;
                value.z = Mathf.Clamp(val, desiredCenterPosition.z - forwardLimit, desiredCenterPosition.z + forwardLimit);
                return value;
            }

            public void OnDisable()
            {
                desiredPosition = DefaultPosition;
            }
        }

        public class DelaySetupMeshCollider : MonoBehaviour
        {
            public float delayInSeconds;
            public MeshCollider meshCollider;
            public Mesh meshToBind;
            private float stopwatch = 0f;

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
            //Debug.Log(sceneNameList.Count);
            return LAISceneManager.sceneNameList;
        }
    }

    public static class HookMethods
    {
        public static void Hook_UI_ShowFade(bool value)
        {
            UI_ShowFade.Value = value;
            CharSelUITransform.Find("BottomSideFade").gameObject.SetActive(value);
            CharSelUITransform.Find("TopSideFade").gameObject.SetActive(value);
        }

        public static void Hook_DisableShaking(bool value)
        {
            Shaking.Value = value;
            var shaker = UnityEngine.Object.FindObjectOfType<PreGameShakeController>();
            if (shaker)
                shaker.enabled = value;
        }

        public static void Hook_UI_BlurOpacity(int value)
        {
            var clampedValue = Mathf.Clamp(value, 0f, 100);
            ConfigSetup.UI_BlurOpacity.Value = Mathf.CeilToInt(clampedValue);

            var SafeArea = CharSelUITransform.Find("SafeArea").transform;
            var ui_left = SafeArea.Find("LeftHandPanel (Layer: Main)");
            var ui_right = SafeArea.Find("RightHandPanel");

            float transparencyValue = ConfigSetup.UI_BlurOpacity.Value / 100f;

            var leftBlurColor = ui_left.Find("BlurPanel").GetComponent<TranslucentImage>();
            leftBlurColor.color = new Color(leftBlurColor.color.r,
                leftBlurColor.color.g,
                leftBlurColor.color.b,
                transparencyValue);
            var rightBlurColor = ui_right.Find("RuleVerticalLayout").Find("BlurPanel").GetComponent<TranslucentImage>();
            rightBlurColor.color = new Color(rightBlurColor.color.r,
                rightBlurColor.color.g,
                rightBlurColor.color.b,
                transparencyValue);
            LAILogging.LogMessage($"Transparency Value: {transparencyValue}" +
                $"\nColor transparency: {leftBlurColor.color.a}", LoggingStyle.UserShouldSee);
        }

        public static void Hook_UIScale(float value)
        {
            UI_Scale.Value = value;
            var SafeArea = CharSelUITransform.Find("SafeArea").transform;
            var ui_left = SafeArea.Find("LeftHandPanel (Layer: Main)");
            var ui_right = SafeArea.Find("RightHandPanel");

            ui_left.localScale = Vector3.one * value;
            ui_right.localScale = Vector3.one * value;
        }

        public static void Hook_Overlay_ShowPostProcessing(bool value)
        {
            PostProcessing.Value = value;
            if (!LAISceneManager.sceneInstance) return;
            var obj = LAISceneManager.sceneInstance.transform.Find("PP");
            if (obj) obj.gameObject.SetActive(value);
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
            var directionalLight = LAISceneManager.sceneInstance.transform.Find("Directional Light");
            if (directionalLight)
            {
                directionalLight.GetComponent<FlickerLight>().enabled = Light_Flicker.Value;
            }
        }

        public static void Hook_LightUpdate_Intensity(float intensity)
        {
            Light_Intensity.Value = intensity;
            var directionalLight = LAISceneManager.sceneInstance.transform.Find("Directional Light");
            if (directionalLight)
            {
                directionalLight.GetComponent<Light>().intensity = Light_Intensity.Value;
            }
        }

        public static void Hook_RescalePads(float size)
        {
            MannequinScale.Value = size;
            var contr = UnityEngine.Object.FindObjectOfType<SurvivorMannequinDioramaController>();
            var obj = contr.gameObject;

            if (obj)
            {
                //if (LobbyViewType != StaticValues.LobbyViewType.Zoom) //if Zoom is selected, then this will NRE //here
                obj.transform.localScale = Vector3.one * size;
            }
        }

        public static void Hook_Rotate_Toggle(bool value)
        {
            MannequinEnableLocalTurn.Value = value;

            if (LAICameraManager.CurrentCameraController)
            {
                LAICameraManager.CurrentCameraController.RotateCamera(true);
            }
        }

        public static void Hook_Rotate_Speed(float speed)
        {
            MannequinEnableLocalTurnMultiplier.Value = speed;
            if (LAICameraManager.CurrentCameraController)
            {
                LAICameraManager.CurrentCameraController.AdjustRotateSpeed(speed);
            }
        }

        public static void Hook_HideProps(bool value)
        {
            MeshProps.Value = value;

            foreach (var propName in Lobby.MeshPropNames)
            {
                var obj = GameObject.Find(propName);
                if (obj) obj.SetActive(MeshProps.Value);
            }
            Hook_HidePhysicsProps(PhysicsProps.Value);
        }

        public static void Hook_HidePhysicsProps(bool value)
        {
            PhysicsProps.Value = value;

            if (!(LAISceneManager.chosenScene is Lobby lobby))
                return;

            if (!Lobby.MeshPropsRef)
            {
                LAILogging.LogWarning($"Hook_HidePhysicsProps: Missing MeshPropsRef for Lobby scene", LoggingStyle.UserShouldSee);
                return;
            }
            var meshPropHolder = Lobby.MeshPropsRef.transform;
            if (meshPropHolder)
            {
                foreach (var propName in Lobby.PhysicsPropNames)
                {
                    var obj = meshPropHolder.Find(propName);
                    if (obj) obj.gameObject.SetActive(value);
                }
            }
        }

        public static void Hook_Overlay_Parallax(bool value)
        {
            Parallax.Value = value;

            if (LAICameraManager.CurrentCameraController)
            {
                LAICameraManager.CurrentCameraController.GetDesiredPositionFromScreenFraction(true);
            }
        }

        public static void Hook_ToggleSceneHeaderVisibility(bool value)
        {
            Scene_Header.Value = value;

            if (LAISceneManager.chosenScene == null) return;
            LAISceneManager.chosenScene.TitleInstance.SetActive(value);
            LAISceneManager.chosenScene.SubTitleInstance.SetActive(value);
        }

        public static void Hook_BlackenSurvivors(bool value)
        {
            SIL_LockedCharactersBlack.Value = value;
            var comps = InstanceTracker.GetInstancesList<Methods.LAI_CharDisplayTracker>();
            if (comps == null || comps.Count == 0) return;

            foreach (var tracker in comps)
            {
                if (tracker)
                    tracker.ToggleShadow(value);
            }
        }

        public static void Hook_ToggleZooming(bool value)
        {
            SIL_ZoomEnable.Value = value;
            if (value)
            {
                UserProfile.onSurvivorPreferenceChangedGlobal += UserProfile_onSurvivorPreferenceChangedGlobal;
                //_logger.LogMessage("Hook'd");
            }
            else
            {
                UserProfile.onSurvivorPreferenceChangedGlobal -= UserProfile_onSurvivorPreferenceChangedGlobal;
                Methods.SetCamera();
                //_logger.LogMessage("Unhooked");
            }
        }

        private static void UserProfile_onSurvivorPreferenceChangedGlobal(UserProfile userProfile)
        {
            var cameraRig = GameObject.Find("Main Camera").GetComponent<CameraRigController>();
            var bodyName = BodyCatalog.GetBodyName(SurvivorCatalog.GetBodyIndexFromSurvivorIndex(userProfile.GetSurvivorPreference().survivorIndex));

            //_logger.LogMessage($"Body Name: {bodyName}");
            // Error here on any_empty
            if (LAILayoutManager.chosenLayout != null && LAILayoutManager.chosenLayout.CharacterCameraSettings != null)
            {
                if (LAILayoutManager.chosenLayout.CharacterCameraSettings.TryGetValue(bodyName, out CharSceneLayout.CameraSetting cameraSetting))
                {
                    Methods.SetCamera(cameraRig, cameraSetting);
                }
                else
                {
                    Methods.SetCamera(cameraRig);
                }
            }
        }
    }
}