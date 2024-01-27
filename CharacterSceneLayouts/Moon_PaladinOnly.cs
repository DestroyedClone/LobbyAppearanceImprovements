﻿using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace LobbyAppearanceImprovements.CharacterSceneLayouts
{
    public class Moon_PaladinOnly : CharSceneLayout
    {
        public override string SceneLayout => "Moon_PaladinOnly";
        public override string SceneName => "Moon";
        public override string Author => "DestroyedClone";
        public override string LayoutName => "Paladin's Entrance";
        public override string ReadmeDescription => "Based on the logo.";
        public override string[] RequiredModGUID => new string[] { "com.rob.Paladin" };

        public override Dictionary<string, Vector3[]> CharacterLayouts => new Dictionary<string, Vector3[]>()
        {
        };

        // these fucks show up in the scene anyway but they get deleted after so who cares
        public static GameObject StatueHolders { get; set; }

        public static RuntimeAnimatorController gamingAnimatorController;

        //https://stackoverflow.com/a/59359043
        public Vector3[][] statuePositionsAndRotations = new Vector3[][]
        {
            new Vector3[]{ new Vector3(1.5f, 0.5f, 8f), new Vector3(0f, 270f, 0f) },
            new Vector3[]{ new Vector3(-1.5f, 0.5f, 8f), new Vector3(0f, 90f, 0f) },

            new Vector3[]{ new Vector3(1.5f, 0.5f, 16f), new Vector3(0f, 270f, 0f) },
            new Vector3[]{ new Vector3(-1.5f, 0.5f, 16f), new Vector3(0f, 90f, 0f) },

            new Vector3[]{ new Vector3(1.5f, 0.5f, 24f), new Vector3(0f, 270f, 0f) },
            new Vector3[]{ new Vector3(-1.5f, 0.5f, 24f), new Vector3(0f, 90f, 0f) },

            new Vector3[]{ new Vector3(1.5f, 0.5f, 32f), new Vector3(0f, 270f, 0f) },
            new Vector3[]{ new Vector3(-1.5f, 0.5f, 32f), new Vector3(0f, 90f, 0f) },
        };

        public override List<GameObject> CreateAdditionalObjectsOnLoad()
        {
            if (!StatueHolders)
            {
                Init();
            }
            if (SurvivorCatalog.FindSurvivorIndex("RobPaladin") < 0)
            {
                Debug.Log("Unable to load scene due to missing survivor Paladin");
                return null;
            }
            List<GameObject> list = new List<GameObject>();

            var displayModel = Methods.CreateDisplay("RobPaladin", new Vector3(0, 0.5f, 5f), new Vector3(0, 180, 0), null, true);
            var animator = displayModel.GetComponent<Animator>();
            animator.runtimeAnimatorController = gamingAnimatorController;
            // warning does not survive the layout being turned off and on, only works on first layout
            EntityStates.EntityState.PlayAnimationOnAnimator(animator, "Body", "Spawn", "Spawn.playbackRate", 3f);
            animator.speed = 0;
            list.Add(StatueHolders);
            list.Add(displayModel);
            return list;
        }

        public override void Init()
        {
            base.Init();
            //if (SurvivorCatalog.FindSurvivorIndex("RobPaladin") < 0) return;
            SetupStatue();
        }

        [RoR2.SystemInitializer(dependencies: typeof(RoR2.SurvivorCatalog))]
        public void SetupStatue()
        {
            if (SurvivorCatalog.FindSurvivorIndex("RobPaladin") < 0)
            {
                Debug.Log("Unable to init scene");
                return;
            }
            Debug.Log("Selecting Material");
            var moonTexture = Resources.Load<GameObject>("prefabs/stagedisplay/MoonDioramaDissplay").transform.Find("MoonBridgeCornerWithTerrain/Terrain").GetComponent<MeshRenderer>().sharedMaterial;

            Debug.Log("Creating Statue");
            var model = Methods.CreateDisplay("RobPaladin", Vector3.zero, Vector3.zero);
            model.GetComponent<CharacterModel>().enabled = false;
            model.transform.Find("Armature/meshPaladin").gameObject.GetComponent<SkinnedMeshRenderer>().material = moonTexture;
            model.transform.Find("Armature/meshSword").gameObject.GetComponent<SkinnedMeshRenderer>().material = moonTexture;
            model.transform.Find("Armature/spine").gameObject.SetActive(false);
            model.name = "PaladinStatue";
            //model.GetComponent<Animator>().playbackTime = 5;
            model.transform.position = new Vector3(0, -50f, 0);
            //model.GetComponent<Animator>();

            var localStatueHolders = new GameObject();
            localStatueHolders.name = "HOLDER: Statues";

            foreach (var statuePos in statuePositionsAndRotations)
            {
                var newModel = GameObject.Instantiate<GameObject>(model);
                newModel.transform.parent = localStatueHolders.transform;
                newModel.transform.position = statuePos[0];
                newModel.transform.rotation = Quaternion.Euler(statuePos[1]);
            }
            StatueHolders = localStatueHolders;
            gamingAnimatorController = SurvivorCatalog.GetSurvivorDef(SurvivorCatalog.FindSurvivorIndex("RobPaladin")).bodyPrefab?.GetComponentInChildren<Animator>().runtimeAnimatorController;
        }
    }
}