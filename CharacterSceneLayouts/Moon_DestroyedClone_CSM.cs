using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;

namespace LobbyAppearanceImprovements.CharacterSceneLayouts
{
    public class Moon_DestroyedClone_CSM : CharSceneLayout
    {
        public override string SceneLayout => "Moon_DestroyedClone_CSM";
        public override string SceneName => "Moon";
        public override string Author => "DestroyedClone";
        public override string LayoutName => "Welcome to Hell";
        public override Dictionary<string, Vector3[]> CharacterLayouts => new Dictionary<string, Vector3[]>()
        {
        };

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
            if (SurvivorCatalog.FindSurvivorIndex("RobPaladin") < 0) return null;

            List<GameObject> list = new List<GameObject>();

            var moonTexture = GameObject.Find("MoonDioramaDissplay(Clone)/MoonBridgeCornerWithTerrain/Terrain").GetComponent<MeshRenderer>().sharedMaterial;
            var model = Methods.CreateDisplay("Commando", new Vector3(0, 0.5f, 5f), new Vector3(0,180,0));
            model.GetComponent<CharacterModel>().enabled = false;
            model.GetComponent<Animator>().speed = 99;
            model.transform.Find("meshPaladin").gameObject.GetComponent<SkinnedMeshRenderer>().material = moonTexture;
            model.transform.Find("meshSword").gameObject.GetComponent<SkinnedMeshRenderer>().material = moonTexture;
            model.transform.Find("Armature").gameObject.SetActive(false);
            model.name = "PaladinStatue";
            //PlayAnimationOnAnimator(anim, "Body", "SpawnClay", "Spawn.playbackRate", 3f);
            //EntityStates.EntityState.PlayAnimationOnAnimator(anim, "Body", "SpawnClay", "Spawn.playbackRate", 3f);

            foreach (var statuePos in statuePositionsAndRotations)
            {
                var newModel = GameObject.Instantiate<GameObject>(model);
                list.Add(newModel);
                newModel.transform.position = statuePos[0];
                newModel.transform.rotation = Quaternion.Euler(statuePos[1]);
            }
            GameObject.Destroy(model);
            var displayModel = Methods.CreateDisplay("RobPaladin", new Vector3(0, 0.5f, 5f), new Vector3(0, 180, 0), null, true);
            var animator = displayModel.GetComponent<Animator>();

            // warning does not survive the layout being turned off and on, only works on first layout
            animator.runtimeAnimatorController = SurvivorCatalog.GetSurvivorDef(SurvivorCatalog.FindSurvivorIndex("RobPaladin")).bodyPrefab?.GetComponentInChildren<Animator>().runtimeAnimatorController;
            EntityStates.EntityState.PlayAnimationOnAnimator(animator, "Body", "Spawn", "Spawn.playbackRate", 3f);
            animator.speed = 0;
            list.Add(displayModel);
            return list;
        }
    }
}
