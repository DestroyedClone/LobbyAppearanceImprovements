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

        public static GameObject PaladinStatue;
        public static Material moonTexture;

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

        public override List<GameObject> CreateAdditionalObjects()
        {
            if (!PaladinStatue)
            {
                Init();
            }
            if (SurvivorCatalog.FindSurvivorIndex("RobPaladin") < 0) return null;

            List<GameObject> list = new List<GameObject>();

            //PlayAnimationOnAnimator(anim, "Body", "SpawnClay", "Spawn.playbackRate", 3f);
            //EntityStates.EntityState.PlayAnimationOnAnimator(anim, "Body", "SpawnClay", "Spawn.playbackRate", 3f);

            foreach (var statuePos in statuePositionsAndRotations)
            {
                var newModel = GameObject.Instantiate<GameObject>(PaladinStatue);
                list.Add(newModel);
                newModel.transform.position = statuePos[0];
                newModel.transform.rotation = Quaternion.Euler(statuePos[1]);
            }
            var displayModel = Methods.CreateDisplay("RobPaladin", new Vector3(0, 0.5f, 5f), new Vector3(0, 180, 0), null, true);
            var animator = displayModel.GetComponent<Animator>();

            // warning does not survive the layout being turned off and on, only works on first layout
            animator.runtimeAnimatorController = SurvivorCatalog.GetSurvivorDef(SurvivorCatalog.FindSurvivorIndex("RobPaladin")).bodyPrefab?.GetComponentInChildren<Animator>().runtimeAnimatorController;
            EntityStates.EntityState.PlayAnimationOnAnimator(animator, "Body", "Spawn", "Spawn.playbackRate", 3f);
            animator.speed = 0;
            list.Add(displayModel);
            return list;
        }

        public override void Init()
        {
            base.Init();
            SetupStatue();
        }

        public void SetupStatue()
        {
            Debug.Log("");
            moonTexture = GameObject.Find("MoonDioramaDissplay(Clone)/MoonBridgeCornerWithTerrain/Terrain").GetComponent<MeshRenderer>().sharedMaterial;

            GameObject model = Methods.CreateDisplay("RobPaladin", Vector3.zero, Vector3.zero);
            model.GetComponent<CharacterModel>().enabled = false;
            model.GetComponent<Animator>().speed = 99;
            model.transform.Find("meshPaladin").gameObject.GetComponent<SkinnedMeshRenderer>().material = moonTexture;
            model.transform.Find("meshSword").gameObject.GetComponent<SkinnedMeshRenderer>().material = moonTexture;
            model.transform.Find("Armature").gameObject.SetActive(false);
            model.name = "PaladinStatue";
            PaladinStatue = model;
        }
    }
}
