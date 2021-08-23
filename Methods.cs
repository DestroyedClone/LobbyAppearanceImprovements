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
            if (addCollider)
            {
                var comp = gameObject.AddComponent<CapsuleCollider>();
                comp.radius = 1f;
            }
            if (LockedCharactersBlack.Value)
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
            if (true) //ClickOnCharacterToSwap
            {
                var com = gameObject.AddComponent<MouseOverAddToList>();
                com.survivorDef = survivorDef;
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
                    GameObject.Find("HANDTeaser").SetActive(false);
                    break;
            }
            return gameObject;
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
                UnityEngine.Object.Destroy(sceneInstance);

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

        public class MouseOverAddToList : MonoBehaviour
        {
            //CapsuleCollider capsuleCollider;
            BoxCollider boxCollider;
            public SurvivorDef survivorDef;
            public Highlight highlight;
            public bool survivorUnlocked = false;
            public LocalUser localUser;
            CharacterSelectController characterSelectController;

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
                    case "Paladin":
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
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    if (!survivorUnlocked)
                        return;
                    characterSelectController.SelectSurvivor(survivorDef.survivorIndex);
                    characterSelectController.SetSurvivorInfoPanelActive(true);
                    localUser.currentNetworkUser?.CallCmdSetBodyPreference(BodyCatalog.FindBodyIndex(survivorDef.bodyPrefab));
                    return;
                }
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
        public class ClickToSetFirstEntryAsChar : MonoBehaviour
        {
            public RoR2.UI.CharacterSelectController characterSelectController;
            public LocalUser localUser;
            public GameObject sceneCamera;
            public float movementModifier = 0.4f;
            public Vector3 startingPosition;

            private Vector3 desiredPosition;
            private Vector3 velocity;
            public float screenLimitDistance = 5f;
            private Vector3 screenLimit;

            public void Awake()
            {
                var screenLimitHalved = screenLimitDistance / 2;
                screenLimit = startingPosition + new Vector3(screenLimitHalved, screenLimitHalved, screenLimitHalved);

                localUser = ((MPEventSystem)EventSystem.current).localUser;
                sceneCamera = GameObject.Find("Main Camera/Scene Camera");
                startingPosition = sceneCamera.transform.position;
                desiredPosition = startingPosition;
            }

            public void Update()
            {
                var w = Input.GetKey(KeyCode.W) ? +1 : 0;
                var a = Input.GetKey(KeyCode.A) ? -1 : 0;
                var s = Input.GetKey(KeyCode.S) ? -1 : 0;
                var d = Input.GetKey(KeyCode.D) ? 1 : 0;
                var q = Input.GetKey(KeyCode.Q) ? -1 : 0;
                var e = Input.GetKey(KeyCode.E) ? 1 : 0;
                var space = Input.GetKey(KeyCode.Space);
                var mult = movementModifier;

                desiredPosition += new Vector3(
                    mult * (a + d),
                    mult * (q + e),
                    mult * (w + s)
                    );

                var horzClamp = Mathf.Clamp(desiredPosition.x, -screenLimit.x, screenLimit.x);
                var vertClamp = Mathf.Clamp(desiredPosition.y, -screenLimit.y, screenLimit.y);
                var forwClamp = Mathf.Clamp(desiredPosition.z, -screenLimit.z, screenLimit.z);


                desiredPosition = new Vector3(
                    horzClamp,
                    vertClamp,
                    forwClamp
                    );
                
                if (space) desiredPosition = startingPosition;

                desiredPosition = dicks();

                DampPosition();
            }

            public void DampPosition()
            {
                sceneCamera.transform.position = Vector3.SmoothDamp(sceneCamera.transform.position, desiredPosition, ref velocity, 0.4f, float.PositiveInfinity, Time.deltaTime);
            }

            public Vector3 MouseVersusCenterScreen()
            {
                Vector3 mousePos = Input.mousePosition;
                var center = new Vector3(Screen.width / 2, Screen.height / 2);
                var value = new Vector3();

                if (mousePos.x < center.x) value.x = -1;
                else if (mousePos.x == center.x) value.x = 0;
                else value.x = 1;

                if (mousePos.y < center.y) value.y = -1;
                else if (mousePos.y == center.y) value.y = 0;
                else value.y = 1;

                var fractionX = (Screen.width - mousePos.x) / Screen.width;
                var fractionY = (Screen.height - mousePos.y) / Screen.height;
                return Vector3.zero;
            }

            public Vector3 cocks()
            {
                Vector3 mousePos = Input.mousePosition;
                var center = new Vector3(Screen.width / 2, Screen.height / 2);
                var value = new Vector3();


                //x
                // left
                if (mousePos.x < center.x) value.x = -(center.x - mousePos.x) / center.x;
                // center
                else if (Mathf.Abs(mousePos.x - center.x) < 0.001) value.x = 0;
                //right
                else value.x = (Screen.width - mousePos.x) / Screen.width;
                return Vector3.zero;
            }

            public Vector3 dicks()
            {
                Vector3 mousePos = Input.mousePosition;
                var center = new Vector3(Screen.width / 2, Screen.height / 2);
                var value = new Vector3();

                float fractionX = (Screen.width - mousePos.x) / Screen.width;
                float fractionY = (Screen.height - mousePos.y) / Screen.height;

                value.x = Mathf.Lerp(startingPosition.x + screenLimitDistance, startingPosition.x - screenLimitDistance, fractionX);
                value.y = Mathf.Lerp(startingPosition.y + screenLimitDistance, startingPosition.y - screenLimitDistance, fractionY);
                return value;
            }
        }
    }
}