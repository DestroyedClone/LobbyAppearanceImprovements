using R2API;
using RoR2;
using System;
using UnityEngine;

namespace LobbyAppearanceImprovements
{
    public static class SceneSetup
    {
        #region ActionSetup

        public static Action<GameObject[]> SceneAssetAPI_IntroAction;
        public static Action<GameObject[]> SceneAssetAPI_LobbyAction;
        public static Action<GameObject[]> SceneAssetAPI_TitleAction;
        public static Action<GameObject[]> SceneAssetAPI_VoidOutroAction;
        public static Action<GameObject[]> SceneAssetAPI_itmoonAction;

        public static void Init()
        {
            SceneAssetAPI_IntroAction += CaptainHelm_Setup;
            SceneAssetAPI.AddAssetRequest("intro", SceneAssetAPI_IntroAction);
            //SceneAssetAPI_LobbyAction += SceneAssetAPI_GetLobbyObjects;
            //SceneAssetAPI.AddAssetRequest("lobby", SceneAssetAPI_LobbyAction);
            //SetupContactLight();

            SceneAssetAPI_TitleAction += LobbyMulti_Setup;
            SceneAssetAPI.AddAssetRequest("title", SceneAssetAPI_TitleAction);

            SceneAssetAPI_VoidOutroAction += VoidOceanFloor_Setup;
            SceneAssetAPI.AddAssetRequest("voidoutro", SceneAssetAPI_VoidOutroAction);

            SceneAssetAPI_itmoonAction += MoonSetup;
            SceneAssetAPI.AddAssetRequest("itmoon", SceneAssetAPI_itmoonAction);
        }

        public static GameObject brotherConstellation;
        private static void MoonSetup(GameObject[] obj)
        {
            foreach (var go in obj)
            {
                if (go.name == "HOLDER: Stage")
                {
                    brotherConstellation = PrefabAPI.InstantiateClone(go.transform.Find("HOLDER: Constellations/mdlBrotherConstellation").gameObject, "LAI_MITHRIXCONSTELLATION", false);
                    return;
                }
            }
        }

        public static GameObject VoidOutroSet7;
        private static void VoidOceanFloor_Setup(GameObject[] obj)
        {
            foreach (var go in obj)
            {
                if (go.name == "Set 7: Bottom of the Ocean")
                {
                    VoidOutroSet7 = PrefabAPI.InstantiateClone(go, "LAI_VOIDOUTROSET7", false);
                    VoidOutroSet7.transform.Find("CrabHolder").GetComponent<ShakeEmitter>().enabled = false;
                    VoidOutroSet7.gameObject.SetActive(true);
                    return;
                }
            }
        }

        public static GameObject SpaceCabin;
        private static void LobbyMulti_Setup(GameObject[] gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.name == "MainMenu")
                {
                    SpaceCabin = PrefabAPI.InstantiateClone(gameObject.transform.Find("MENU: Multiplayer/World Position/HOLDER: Background").gameObject, "LAI_SPACECABIN", false);
                    //https://forum.unity.com/threads/changing-objects-to-from-static-at-runtime.575080/#post-7140668
                    SpaceCabin.isStatic = false;
                    foreach (var tool in SpaceCabin.transform.GetComponentsInChildren<Transform>())
                    {
                        tool.gameObject.isStatic = false;
                    }
                    return;
                }
            }
        }



        #endregion ActionSetup

        // Scene Prefabs

        #region Captain's Helm Setup

        public static GameObject CaptainHelmObject;

        public static void CaptainHelm_Setup(GameObject[] gameObjects)
        {
            GameObject cabin = null;
            GameObject skybox = null;
            GameObject planet = null;
            foreach (var gameObject in gameObjects)
            {
                switch (gameObject.name)
                {
                    case "Set 2 - Cabin":
                        cabin = gameObject;
                        continue;
                    case "Cutscene Space Skybox":
                        skybox = gameObject;
                        continue;
                    case "Set 3 - Space, Small Planet":
                        planet = gameObject;
                        planet.SetActive(false); //for now
                        continue;
                }
            }
            var newHolder = new GameObject();
            cabin.transform.parent = newHolder.transform;
            cabin.transform.position = new Vector3(1f, 1f, 1f);
            cabin.transform.Find("CabinPosition").transform.localScale = Vector3.one * 200;
            cabin.transform.Find("CabinPosition").transform.localPosition = new Vector3(-18, 4.2f, 6);

            skybox.transform.parent = newHolder.transform;
            skybox.transform.localScale = Vector3.one * 30;
            planet.transform.parent = newHolder.transform;
            //planet.SetActive(true);
            CaptainHelmObject = PrefabAPI.InstantiateClone(newHolder, "LAI_CaptainsHelm_Cabin");
            UnityEngine.Object.Destroy(newHolder);
        }

        #endregion Captain's Helm Setup

        #region UES Contact Light Setup

        public static void SceneAssetAPI_GetLobbyObjects(GameObject[] gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.name == "MeshProps")
                {
                    var backWall = GameObject.Find("spacecabin");

                    CaptainHelmObject = PrefabAPI.InstantiateClone(backWall, "Cabin", false);
                }
            }
        }

        public static GameObject ContactLightPrefab;
        public static GameObject ContactLight_RestraintBar;
        public static GameObject ContactLight_Commando;
        public static GameObject ContactLight_Enforcer;
        public static GameObject ContactLight_Bandit2;
        public static GameObject ContactLight_Huntress;
        public static GameObject ContactLight_HAND;

        public static GameObject ContactLight_Engineer;
        public static GameObject ContactLight_Miner;
        public static GameObject ContactLight_Sniper;
        public static GameObject ContactLight_Acrid;
        public static GameObject ContactLight_Mercenary;
        public static GameObject ContactLight_Loader;
        public static GameObject ContactLight_CHEF;

        public static void SetupContactLight()
        {
            var restraintMesh = RoR2Content.Survivors.Croco.displayPrefab.transform.Find("mdlCroco/Spawn/SpawnFX/Chunks, Solid").GetComponent<ParticleSystemRenderer>().mesh;
            var obj = new GameObject();

            ContactLightPrefab = PrefabAPI.InstantiateClone(obj, "Contact Light Prefab", false);
            ContactLight_RestraintBar = new GameObject();
            ContactLight_RestraintBar = PrefabAPI.InstantiateClone(obj, "RestraintBar", false);
            var meshRenderer = ContactLight_RestraintBar.AddComponent<MeshRenderer>();
            var meshFilter = ContactLight_RestraintBar.AddComponent<MeshFilter>();
            meshFilter.mesh = restraintMesh;

            SetupCommando();
            SetupBandit2();

            UnityEngine.Object.Destroy(obj);
        }

        public static void SetupCommando()
        {
            ContactLight_Commando = PrefabAPI.InstantiateClone(RoR2Content.Survivors.Commando.displayPrefab, "ContactLight_Commando", false);
            ContactLight_Commando.transform.localEulerAngles = new Vector3(0, 180, 0);
            ContactLight_Commando.transform.position = new Vector3(0f, 0f, 2f);
            var restraintBar = ContactLight_RestraintBar.InstantiateClone("Commando_RestraintBar", false);
            restraintBar.transform.SetParent(ContactLight_Commando.transform);
            restraintBar.transform.localEulerAngles = new Vector3(310, 0, 0);
            restraintBar.transform.localScale = new Vector3(0.25f, 0.6f, 0.4f);
            restraintBar.transform.localPosition = new Vector3(0.02f, 1.4f, 0.2f);
            ContactLight_Commando.GetComponentInChildren<Animator>().enabled = false;
            var chest = ContactLight_Commando.transform.Find("mdlCommandoDualies/CommandoArmature/ROOT/base/stomach/chest");
            chest.Find("upper_arm.l").localEulerAngles = new Vector3(0, 0, 145);
            chest.Find("upper_arm.l/lower_arm.l").localEulerAngles = new Vector3(340, 270, 240);
            chest.Find("upper_arm.l/lower_arm.l/hand.l").localEulerAngles = new Vector3(15, 35, -8.8389f);
            chest.Find("upper_arm.l/lower_arm.l/hand.l/gun.l").localScale = Vector3.zero;
            chest.Find("upper_arm.r").localEulerAngles = new Vector3(3, 4, 205);
            chest.Find("upper_arm.r/lower_arm.r").localEulerAngles = new Vector3(340, 270, 240);
            chest.Find("upper_arm.r/lower_arm.r/hand.r").localEulerAngles = new Vector3(15, 35, -8.8389f);
            chest.Find("upper_arm.r/lower_arm.r/hand.r/gun.r").transform.localScale = Vector3.zero;
        }

        public static void SetupBandit2()
        {
            ContactLight_Bandit2 = PrefabAPI.InstantiateClone(RoR2Content.Survivors.Bandit2.displayPrefab, "ContactLight_Bandit2", false);
            ContactLight_Bandit2.transform.localEulerAngles = new Vector3(0, 180, 0);
            ContactLight_Bandit2.transform.position = new Vector3(0f, 0f, 2f);
            ContactLight_Commando.GetComponentInChildren<Animator>().enabled = false;
            var restraintBar = ContactLight_RestraintBar.InstantiateClone("Commando_RestraintBar", false);
            restraintBar.transform.SetParent(ContactLight_Bandit2.transform);
            restraintBar.transform.localEulerAngles = new Vector3(310, 0, 0);
            restraintBar.transform.localScale = new Vector3(0.25f, 0.6f, 0.4f);
            restraintBar.transform.localPosition = new Vector3(-0.06f, 1.6f, 2.75f);
        }

        #endregion UES Contact Light Setup
    }
}