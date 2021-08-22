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

            public void Start()
            {
                if (survivorDef)
                {
                    highlight = gameObject.AddComponent<Highlight>();
                    highlight.highlightColor = Highlight.HighlightColor.interactive;
                    highlight.isOn = false;
                    highlight.targetRenderer = GetTargetRenderer();
                }
                else
                    Debug.LogWarning("No survivorDef found for " + gameObject.name);

                /*capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
                    capsuleCollider.contactOffset = 0.1f;
                    capsuleCollider.radius = 0.5f;
                    capsuleCollider.height = 1.82f;
                    capsuleCollider.direction = 1;
                capsuleCollider.center = Vector3.up* 1.2f;*/

                SetupBoxCollider();
            }
            //https://stackoverflow.com/questions/62986775/how-can-get-the-size-of-an-object-and-add-boxcollider-that-will-cover-automatic
            public void SetupBoxCollider()
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

            public SkinnedMeshRenderer GetTargetRenderer()
            {
                Debug.Log("Checking cached name " + survivorDef.cachedName);
                string path = "";

                switch (survivorDef.cachedName)
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
                        path = "mdl"+ survivorDef.cachedName + "/"+ survivorDef.cachedName + "Mesh";
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
                return transform?.GetComponent<SkinnedMeshRenderer>();
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

            public void OnMouseEnter()
            {
                mousedOverObjects.Add(survivorDef);
                if (highlight) highlight.isOn = true;
            }
            public void OnMouseExit()
            {
                mousedOverObjects.Remove(survivorDef);
                if (highlight) highlight.isOn = false;
            }
        }
        public class ClickToSetFirstEntryAsChar : MonoBehaviour
        {
            public RoR2.UI.CharacterSelectController characterSelectController;
            public LocalUser localUser;
            public GameObject sceneCamera;
            public float movementModifier = 1f;

            public void Awake()
            {
                localUser = ((MPEventSystem)EventSystem.current).localUser;
                sceneCamera = GameObject.Find("Main Camera/Scene Camera");
            }



            public void Update()
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (mousedOverObjects.Count > 0)
                    {
                        foreach (var chosenSurvivorDef in mousedOverObjects)
                        {
                            if (!SurvivorCatalog.SurvivorIsUnlockedOnThisClient(chosenSurvivorDef.survivorIndex))
                            {
                                continue;
                            }
                            characterSelectController.SelectSurvivor(chosenSurvivorDef.survivorIndex);
                            characterSelectController.SetSurvivorInfoPanelActive(true);
                            localUser.currentNetworkUser?.CallCmdSetBodyPreference(BodyCatalog.FindBodyIndex(chosenSurvivorDef.bodyPrefab));
                            mousedOverObjects.Clear();
                            return;
                        }
                    }
                }

                var w = Input.GetKey(KeyCode.W);
                var a = Input.GetKey(KeyCode.A);
                var s = Input.GetKey(KeyCode.S);
                var d = Input.GetKey(KeyCode.D);
                var q = Input.GetKey(KeyCode.Q);
                var e = Input.GetKey(KeyCode.E);
                //var limit = 10f;
                //var t = sceneCamera.transform.position; ;
                var mult = movementModifier;

                if (w) sceneCamera.transform.Translate(0, 0, mult);
                if (s) sceneCamera.transform.Translate(0, 0, -mult);
                if (a) sceneCamera.transform.Translate(-mult, 0, 0);
                if (d) sceneCamera.transform.Translate(mult, 0, 0);
                if (q) sceneCamera.transform.Translate(0, mult, 0);
                if (e) sceneCamera.transform.Translate(0, -mult, 0);
            }
            public void FixedUpdate()
            {
                if (mousedOverObjects.Count > 0)
                {
                    int i = 0;
                    foreach (var characterMaster in mousedOverObjects)
                    {
                        //Debug.Log(i + " " + Language.GetString(characterMaster.displayNameToken));
                        i++;
                    }
                }
            }
        }
    }
}