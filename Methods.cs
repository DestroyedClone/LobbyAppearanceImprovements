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

        public class LAI_CharDisplayTracker : MonoBehaviour
        {
            public CharacterModel characterModel;
            public bool hasUnlocked = false;

            public void ToggleShadow(bool value)
            {
                if (characterModel)
                    characterModel.isDoppelganger = value && !hasUnlocked;
            }
        }

        public static void SetCamera(CameraRigController cameraRig, CharSceneLayout.CameraSetting cameraSetting)
        {
            SetCamera(cameraRig, cameraSetting.fov, cameraSetting.pitch, cameraSetting.yaw);
            //add pos and rot
        }
        public static void SetCamera(CameraRigController cameraRig = null, float fov = 60f, float pitch = 0f, float yaw = 0f)
        {
            if (!cameraRig)
            {
                cameraRig = GameObject.Find("Main Camera").gameObject.GetComponent<CameraRigController>();

                if (!cameraRig)
                {
                    if (ConfigSetup.ShowLoggingText.Value > LoggingStyle.Minimal)
                    {
                        _logger.LogMessage("Couldn't find CameraRig!");
                    }
                    return;
                }
            }

            var modifier = 0f;
            if (fov != 60f && pitch != 0f && yaw != 0f)
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
            cameraRig.pitch = pitch;
            cameraRig.yaw = yaw;
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
                if (ConfigSetup.ShowLoggingText.Value > ConfigSetup.LoggingStyle.Minimal)
                    _logger.LogMessage("CreateDisplay :: Aborted, no body prefab found for "+bodyPrefabName);
                return null;
            }

            SurvivorDef survivorDef = SurvivorCatalog.FindSurvivorDefFromBody(bodyPrefab);
            if (!survivorDef)
            {
                if (ConfigSetup.ShowLoggingText.Value > ConfigSetup.LoggingStyle.Minimal)
                    _logger.LogMessage("CreateDisplay :: Aborted, no SurvivorDef found for "+bodyPrefabName);
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
            var selectedScene = scenesDict.TryGetValue(sceneName, out var scene);
            if (!selectedScene)
            {
                if (ConfigSetup.ShowLoggingText.Value > ConfigSetup.LoggingStyle.None)
                    _logger.LogWarning($"SelectScene :: {sceneName} not found!");
                return;
            }

            if (sceneInstance)
            {
                chosenScene.OnDestroy();
                UnityEngine.Object.Destroy(sceneInstance);
            }

            MeshPropsRef.SetActive(sceneName == "Lobby");

            var sceneObject = (LAIScene)Activator.CreateInstance(scene);
            chosenScene = sceneObject;
            sceneInstance = sceneObject.CreateScene();
            ConfigSetup.SelectedScene.Value = sceneName;
        }

        public static void SelectLayout(string layoutName, bool saveChanges = true)
        {
            var selectedLayout = layoutsDict.TryGetValue(layoutName, out var layout);
            if (!selectedLayout)
            {
                if (ConfigSetup.ShowLoggingText.Value > ConfigSetup.LoggingStyle.None)
                    _logger.LogWarning($"SelectLayout :: {layoutName} not found!");
                return;
            }

            if (layoutInstance)
                UnityEngine.Object.Destroy(layoutInstance);

            var layoutObject = (CharSceneLayout)Activator.CreateInstance(layout);
            chosenLayout = layoutObject;
            layoutInstance = layoutObject.CreateLayout();
            if (saveChanges)
                ConfigSetup.SIL_SelectedLayout.Value = layoutName;

            //Resets camera on layout change
            var cameraRig = GameObject.Find("Main Camera").gameObject.GetComponent<CameraRigController>();
            SetCamera(cameraRig);
        }

        public static string GetDefaultLayoutNameForScene(string sceneName)
        {
            foreach (var kvp in layoutsDict)
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
            var currentSceneIsNotLobby = sceneName != (string)SelectedScene.DefaultValue;
            var sceneNameForLayout = currentSceneIsNotLobby ? sceneName : "Lobby";

            bool resultScene = false;
            bool resultLayout = false;

            if (currentSceneIsNotLobby)
            {
                if (sceneName != null)
                {
                    if (!scenesDict.ContainsKey(sceneName))
                    {
                        if (ConfigSetup.ShowLoggingText.Value > ConfigSetup.LoggingStyle.None)
                            _logger.LogWarning($"LoadSceneAndLayout :: Could not find scene \"{sceneName}\"!");
                    }
                    else
                    {
                        Methods.SelectScene(sceneName);
                        resultScene = true;
                    }
                }
            }
            if (layoutName == null)
            {
                layoutName = LAIPlugin.chosenScene.PreferredLayout != null ? LAIPlugin.chosenScene.PreferredLayout : nameof(Any_Empty);
            }
            if (SIL_Enabled.Value)
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
                {

                    if (ConfigSetup.ShowLoggingText.Value > ConfigSetup.LoggingStyle.Minimal)
                        _logger.LogWarning("ClickToSelectCharacter :: No SurvivorDef found for " + gameObject.name);
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
                _logger.LogMessage($"ClickToSelectCharacter.GetTargetRenderer :: Checking cached name {cachedName}.");
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
        public class LAICameraController : MonoBehaviour
        {
            public static LAICameraController instance;

            public GameObject sceneCamera;
            // Defaults
            public string sepDefaults = "==Defaults==";
            public Vector3 defaultPosition;
            public Quaternion defaultRotation;
            // Parallax
            public string setpParallax = "==Parallax==";
            public Vector3 desiredPosition; // Desired position for parallax
            private Vector3 velocity;
            private float screenLimitDistance = 0.25f; //Limit of the screen to move with parallax from the center of the screen.
            private float forwardLimit = 5f;
            private float forwardMult = 0.25f;

            // Zoom On Character
            public string sepZoom = "==Zoom==";
            public Vector3 desiredPositionOverride; // Desired position for ZoomOnCharacter

            // Rotating Character
            public string sepRotate = "==Rotate==";
            public Vector3 rotate_initialPosition;
            public Vector3 rotate_currentPosition;
            public float rotate_multiplier = 2f;
            public Vector3 rotate_defaultRotationChar;

            // Other
            public string setpOther = "==Other==";
            bool screenIsFocused = true;
            private Vector3 MousePosition;

            private bool mouse0Click = false;
            public CharacterSelectController characterSelectController;
            public CharacterSelectController.CharacterPad[] characterPads = null;

            public void Awake()
            {
                if (!instance)
                {
                    instance = this;
                } else
                {
                    _logger.LogWarning("Two instances of LAICameraController were spawned?");
                }

                sceneCamera = GameObject.Find("Main Camera/Scene Camera");
                defaultPosition = sceneCamera.transform.position;
                defaultRotation = sceneCamera.transform.rotation;
                if (!characterSelectController)
                {
                    characterSelectController = UnityEngine.Object.FindObjectOfType<CharacterSelectController>();
                    characterPads = characterSelectController.characterDisplayPads;
                    rotate_defaultRotationChar = characterPads[0].padTransform.eulerAngles;
                }

                desiredPosition = defaultPosition;

                if (CurrentCameraController != null && CurrentCameraController != this)
                {
                    _logger.LogWarning("Somehow there are two camera parallaxes?");
                }
                CurrentCameraController = this;
            }

            public void AdjustRotateSpeed(float speed)
            {
                rotate_multiplier = speed;
            }

            public void OnDestroy()
            {
                CurrentCameraController = null;
            }

            public void Update()
            {
                if (screenIsFocused)
                {
                    MousePosition = Input.mousePosition;
                    if (Parallax.Value)
                        desiredPosition = GetDesiredPositionFromScreenFraction();
                    if (TurnCharacter.Value)
                        RotateCamera();
                }

                DampPosition();
            }

            public void RotateCamera(bool reset = false)
            {
                if (reset)
                {
                    rotate_initialPosition = defaultPosition;
                    rotate_currentPosition = defaultPosition;
                    characterPads[0].padTransform.eulerAngles = rotate_defaultRotationChar;
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
                    rotate_initialPosition = defaultPosition;
                    rotate_currentPosition = defaultPosition;
                    characterPads[0].padTransform.eulerAngles = rotate_defaultRotationChar;
                }

                if (characterPads != null)
                {
                    //var rotationVector = Vector3.Distance(rotate_initialPosition, rotate_currentPosition);
                    var rotationVector = rotate_initialPosition.x - rotate_currentPosition.x;
                    //var modifier = rotate_currentPosition.x < rotate_initialPosition.x ? 1 : -1;
                    var newRotation = rotationVector * rotate_multiplier;
                    characterPads[0].padTransform.eulerAngles = rotate_defaultRotationChar + newRotation * Vector3.up;
                    
                }
            }



            void OnApplicationFocus(bool hasFocus)
            {
                screenIsFocused = hasFocus;
            }

            public void DampPosition()
            {
                sceneCamera.transform.position = Vector3.SmoothDamp(sceneCamera.transform.position, desiredPosition, ref velocity, 0.4f, float.PositiveInfinity, Time.deltaTime);
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

                value.x = Mathf.Lerp(defaultPosition.x + screenLimitDistance, defaultPosition.x - screenLimitDistance, fractionX);
                value.y = Mathf.Lerp(defaultPosition.y + screenLimitDistance, defaultPosition.y - screenLimitDistance, fractionY);

                //if (Input.GetMouseButtonDown(2))
                    //value.z = startingPosition.z;

                var val = defaultPosition.z + Input.mouseScrollDelta.y * forwardMult;
                value.z = Mathf.Clamp(val, defaultPosition.z - forwardLimit, defaultPosition.z + forwardLimit);
                return value;
            }

            public void OnDisable()
            {
                desiredPosition = defaultPosition;
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
            //Debug.Log(sceneNameList.Count);
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
            UI_Scale.Value = value;
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
                characterPadAlignments.transform.localScale = Vector3.one * size;

            }
        }

        public static void Hook_Rotate_Toggle(bool value)
        {
            TurnCharacter.Value = value;

            if (CurrentCameraController)
            {
                CurrentCameraController.RotateCamera(true);
            }
        }

        public static void Hook_Rotate_Speed(float speed)
        {
            TurnCharacterMult.Value = speed;
            if (CurrentCameraController)
            {
                CurrentCameraController.AdjustRotateSpeed(speed);
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

            if (CurrentCameraController)
            {
                CurrentCameraController.GetDesiredPositionFromScreenFraction(true);
            }
        }

        public static void Hook_SurvivorsInLobby(bool value) // i have no idea what im doing
        {
            if (value)
            {
                SIL_Enabled.Value = value; //order matters
                // Needs to be set before so the fucking method can change it back
                Methods.LoadSceneAndLayout(SelectedScene.Value, SIL_SelectedLayout.Value);
            } else
            {
                Methods.LoadSceneAndLayout(null, nameof(Any_Empty), false);
                SIL_Enabled.Value = value;
                // needs to be set after so the method can change
            }
        }

        public static void Hook_BlackenSurvivors(bool value)
        {
            SIL_LockedCharactersBlack.Value = value;
            var comps = UnityEngine.Object.FindObjectsOfType<Methods.LAI_CharDisplayTracker>();
            if (comps == null || comps.Length == 0) return;

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
                On.RoR2.UI.CharacterSelectController.SelectSurvivor += ZoomOnSelected;
                //_logger.LogMessage("Hook'd");
            } else
            {
                On.RoR2.UI.CharacterSelectController.SelectSurvivor -= ZoomOnSelected;
                Methods.SetCamera();
                //_logger.LogMessage("Unhooked");
            }
        }

        private static void ZoomOnSelected(On.RoR2.UI.CharacterSelectController.orig_SelectSurvivor orig, CharacterSelectController self, SurvivorIndex survivor)
        {
            orig(self, survivor);
            var cameraRig = GameObject.Find("Main Camera").gameObject.GetComponent<CameraRigController>();
            var bodyName = BodyCatalog.GetBodyName(SurvivorCatalog.GetBodyIndexFromSurvivorIndex(survivor));

            //_logger.LogMessage($"Body Name: {bodyName}");
            // Error here on any_empty
            if (chosenLayout != null && chosenLayout.CharacterCameraSettings != null)
            {
                if (LAIPlugin.chosenLayout.CharacterCameraSettings.TryGetValue(bodyName, out CharSceneLayout.CameraSetting cameraSetting))
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