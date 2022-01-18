using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using R2API;
using UnityEngine;


namespace LobbyAppearanceImprovements
{
    public static class SceneSetup
    {
        #region ActionSetup
        public static Action<GameObject[]> SceneAssetAPI_IntroAction;
        public static Action<GameObject[]> SceneAssetAPI_LobbyAction;
        public static void Init()
        {
            SceneAssetAPI_IntroAction += CaptainHelm_Setup;
            SceneAssetAPI.AddAssetRequest("intro", SceneAssetAPI_IntroAction);
            //SceneAssetAPI_LobbyAction += SceneAssetAPI_GetLobbyObjects;
            //SceneAssetAPI.AddAssetRequest("lobby", SceneAssetAPI_LobbyAction);
            //SetupContactLight();
        }
        #endregion


        // Scene Prefabs
        #region Captain's Helm Setup
        public static GameObject CaptainHelmObject;
        public static void CaptainHelm_Setup(GameObject[] gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.name == "Set 2 - Cabin")
                {
                    CaptainHelmObject = PrefabAPI.InstantiateClone(gameObject, "Cabin");
                    return;
                }
            }
        }
        #endregion

        #region UES Contact Light Setup
        public static void SceneAssetAPI_GetLobbyObjects(GameObject[] gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.name == "MeshProps")
                {
                    var backWall = GameObject.Find("spacecabin");

                    CaptainHelmObject = PrefabAPI.InstantiateClone(backWall, "Cabin");
                }
            }
        }
        public static GameObject ContactLightPrefab;
        public static GameObject ContactLight_RestraintBar;
        public static GameObject ContactLight_Commando;
        public static void SetupContactLight()
        {
            var restraintMesh = RoR2Content.Survivors.Croco.displayPrefab.transform.Find("mdlCroco/Spawn/SpawnFX/Chunks, Solid").GetComponent<ParticleSystemRenderer>().mesh;
            ContactLight_RestraintBar = new GameObject();
            var meshRenderer = ContactLight_RestraintBar.AddComponent<MeshRenderer>();
            var meshFilter = ContactLight_RestraintBar.AddComponent<MeshFilter>();
            meshFilter.mesh = restraintMesh;
            ContactLight_RestraintBar = PrefabAPI.InstantiateClone(ContactLight_RestraintBar, "RestraintBar");
            SetupCommando();


        }

        public static void SetupCommando()
        {
            ContactLight_Commando = PrefabAPI.InstantiateClone(RoR2Content.Survivors.Commando.displayPrefab, "ContactLight_Commando");
            ContactLight_Commando.transform.localEulerAngles = new Vector3(0, 180, 0);
            ContactLight_Commando.transform.position = new Vector3(0f, 0f, 2f);
            var restraintBar = ContactLight_RestraintBar.InstantiateClone("Commando_RestraintBar").transform.parent = ContactLight_Commando.transform;
            restraintBar.transform.localEulerAngles = new Vector3(310, 0, 0);
            restraintBar.transform.localScale = new Vector3(0.25f, 0.6f, 0.4f);
            restraintBar.transform.localPosition = new Vector3(0.02f, 1.4f, 0.2f);
            ContactLight_Commando.GetComponentInChildren<Animator>().enabled = false;
            var chest = ContactLight_Commando.transform.Find("mdlCommandoDualies/CommandoArmature/ROOT/base/stomach/chest");
            chest.Find("upper_arm.l").localEulerAngles = new Vector3(0, 0, 145);
            chest.Find("upper_arm.l/lower_arm.l").localEulerAngles = new Vector3(340, 270, 240);
            chest.Find("upper_arm.l/lower_arm.l/hand.l").localEulerAngles = new Vector3(15, 35, -8.8389f);
            chest.Find("upper_arm.l/lower_arm.l/gun.l").transform.localScale = Vector3.zero;
            chest.Find("upper_arm.r").localEulerAngles = new Vector3(3, 4, 205);
            chest.Find("upper_arm.r/lower_arm.r").localEulerAngles = new Vector3(340, 270, 240);
            chest.Find("upper_arm.r/lower_arm.r/hand.r").localEulerAngles = new Vector3(15, 35, -8.8389f);
            chest.Find("upper_arm.r/lower_arm.r/gun.r").transform.localScale = Vector3.zero;
        }
        #endregion

    }
}
