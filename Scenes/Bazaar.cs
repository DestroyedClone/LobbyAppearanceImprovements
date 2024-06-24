using System;
using UnityEngine;
using System.Collections.Generic;
using RoR2;

namespace LobbyAppearanceImprovements.Scenes
{
    public class Bazaar : LAIScene
    {
        public override string SceneNameToken => "MAP_BAZAAR";
        public override string SeerToken => "LAI_SEER_BAZAAR";
        public override GameObject BackgroundPrefab => SceneSetup.BazaarStoreObject;
        public override Vector3 Position => new Vector3(-3f, 15f, 18);
        public override Quaternion Rotation => Quaternion.Euler(0f, 150f, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/DLC1/ancientloft/matSkyboxAncientLoft.mat");
        public override string MusicTrackName => "muSong04";

        public override void Init()
        {
            base.Init();
            LAIScene.onSceneLoaded += OnSceneLoaded;
            LAIScene.onSceneUnloaded += OnSceneUnloaded;
            On.RoR2.NetworkStateMachine.OnSerialize += NetworkStateMachine_OnSerialize;
        }

        private void OnSceneLoaded(LAIScene scene)
        {
            if (!scene.IsSceneOfType<Bazaar>()) return;
            //LAISceneManager.sceneInstance.GetComponent<LAIBazaarController>().ReloadDisplays();
            On.RoR2.PurchaseInteraction.OnEnable += PurchaseInteraction_OnEnable;
        }

        private void PurchaseInteraction_OnEnable(On.RoR2.PurchaseInteraction.orig_OnEnable orig, PurchaseInteraction self)
        {
            return;
        }

        private void OnSceneUnloaded(LAIScene scene)
        {
            if (!scene.IsSceneOfType<Bazaar>()) return;
            On.RoR2.PurchaseInteraction.OnEnable -= PurchaseInteraction_OnEnable;
        }

        private bool NetworkStateMachine_OnSerialize(On.RoR2.NetworkStateMachine.orig_OnSerialize orig, NetworkStateMachine self, UnityEngine.Networking.NetworkWriter writer, bool initialState)
        {
            if (!Run.instance)
                return false;
            return orig(self, writer, initialState);
        }

        public class LAIBazaarController : MonoBehaviour
        {
            public List<PickupDisplay> displays = new List<PickupDisplay>();
            public List<string> relativePaths = new List<string>();

            public void ReloadDisplays()
            {
                displays = InstanceTracker.GetInstancesList<PickupDisplay>();
                var list = Run.instance.availableLunarCombinedDropList;

                foreach (var display in displays)
                {
                    display.SetPickupIndex(list[UnityEngine.Random.Range(0, list.Count)].pickupDef.pickupIndex);
                }
            }
        }

        public class PUTracker : MonoBehaviour
        {
            public PickupDisplay pickupDisplay;

            public void Awake()
            {
                InstanceTracker.Add(pickupDisplay);
            }

            public void OnDestroy()
            {
                InstanceTracker.Remove(pickupDisplay);
            }
        }

    }
}