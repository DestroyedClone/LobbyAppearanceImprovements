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
            SceneAssetAPI_LobbyAction += SceneAssetAPI_GetLobbyObjects;
            SceneAssetAPI.AddAssetRequest("lobby", SceneAssetAPI_LobbyAction);
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

                    CaptainHelmObject = PrefabAPI.InstantiateClone(gameObject, "Cabin");
                    return;
                }
            }
        }
        public static GameObject ContactLightPrefab;
        public static GameObject ContactLight_RestraintBar;
        private static void SetupContactLight()
        {
            var restraintMesh = RoR2Content.Survivors.Croco.displayPrefab.transform.Find("mdlCroco/Spawn/SpawnFX/Chunks, Solid").GetComponent<ParticleSystemRenderer>().mesh;
            ContactLight_RestraintBar = new GameObject();
            var meshRenderer = ContactLight_RestraintBar.AddComponent<MeshRenderer>();
            var meshFilter = ContactLight_RestraintBar.AddComponent<MeshFilter>();
            meshFilter.mesh = restraintMesh;
        }
        #endregion

    }
}
