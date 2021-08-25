using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;

namespace LobbyAppearanceImprovements.CharacterSceneLayouts
{
    public class Moon_DestroyedClone_PaladinOnly : CharSceneLayout
    {
        public override string SceneLayout => "Moon_DestroyedClone_PaladinOnly";
        public override string SceneName => "Moon";
        public override string Author => "DestroyedClone";
        public override string LayoutName => "Paladin's Entrance";
        public override Dictionary<string, Vector3[]> CharacterLayouts => new Dictionary<string, Vector3[]>()
        {
            //{ "RobPaladin", new [] {new Vector3(0f, 0.5f, 5f), new Vector3(0f, 180f, 0f) } },
        };

        public static GameObject StatueHolders;
        public static GameObject PaladinStatue;
        public static Material moonTexture;
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
        };

        public override List<GameObject> CreateAdditionalObjectsOnLoad()
        {
            if (SurvivorCatalog.FindSurvivorIndex("RobPaladin") < 0)
            {
                Debug.Log("Unable to load scene due to missing survivor Paladin");
                return null;
            }
            if (!StatueHolders)
            {
                Init();
            }

            List<GameObject> list = new List<GameObject>();

            // Statues
            //var localStatueHolders = GameObject.Instantiate(StatueHolders);
            //list.Add(localStatueHolders);

            var displayModel = Methods.CreateDisplay("RobPaladin", new Vector3(0, 0.5f, 5f), new Vector3(0, 180, 0), null, true);
            var animator = displayModel.GetComponent<Animator>();
            animator.runtimeAnimatorController = gamingAnimatorController;
            // warning does not survive the layout being turned off and on, only works on first layout
            EntityStates.EntityState.PlayAnimationOnAnimator(animator, "Body", "Spawn", "Spawn.playbackRate", 3f);
            animator.speed = 0;
            //list.Add(displayModel);
            return list;
        }

        public override void Init()
        {
            base.Init();
            if (SurvivorCatalog.FindSurvivorIndex("RobPaladin") < 0) return;
            SetupStatue();
        }

        //[RoR2.SystemInitializer(dependencies: typeof(RoR2.EntityStateCatalog))]
        public void SetupStatue()
        {
            Debug.Log("Selecting Material");
            moonTexture = Resources.Load<GameObject>("prefabs/stagedisplay/MoonDioramaDissplay").transform.Find("MoonBridgeCornerWithTerrain/Terrain").GetComponent<MeshRenderer>().sharedMaterial;

            Debug.Log("Creating Stature");
            GameObject model = Methods.CreateDisplay("RobPaladin", Vector3.zero, Vector3.zero);
            model.GetComponent<CharacterModel>().enabled = false;
            model.GetComponent<Animator>().speed = 99;
            model.transform.Find("meshPaladin").gameObject.GetComponent<SkinnedMeshRenderer>().material = moonTexture;
            model.transform.Find("meshSword").gameObject.GetComponent<SkinnedMeshRenderer>().material = moonTexture;
            model.transform.Find("Armature").gameObject.SetActive(false);
            model.name = "PaladinStatue";
            PaladinStatue = model;

            var localStatueHolders = new GameObject();
            localStatueHolders.name = "HOLDER: Statues";

            foreach (var statuePos in statuePositionsAndRotations)
            {
                var newModel = GameObject.Instantiate<GameObject>(PaladinStatue);
                newModel.transform.parent = localStatueHolders.transform;
                newModel.transform.position = statuePos[0];
                newModel.transform.rotation = Quaternion.Euler(statuePos[1]);
            }
            StatueHolders = localStatueHolders;

            Debug.Log("Getting anim controller");
            gamingAnimatorController = SurvivorCatalog.GetSurvivorDef(SurvivorCatalog.FindSurvivorIndex("RobPaladin")).bodyPrefab?.GetComponentInChildren<Animator>().runtimeAnimatorController;

        }
    }
}
