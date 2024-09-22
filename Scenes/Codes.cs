using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace LobbyAppearanceImprovements.Scenes
{
    public class Codes : LAIScene
    {
        public override string SceneNameToken => "LAI_MAP_CODES";
        public override string SeerToken => "LAI_SEER_CODES";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new Vector3(17f, -26.2f, 107f);
        public override Quaternion Rotation => Quaternion.Euler(0, 165, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/skymeadow/matSMSkybox.mat");
        public override string MusicTrackName => "muSong14";

        //RoR2/Base/skymeadow/matSMSkybox.mat
        //RoR2/Base/skymeadow/matSMSkyboxMoon.mat
        public static GameObject display;

        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/Base/skymeadow/PortalDialerEvent.prefab", "LAI_Scene_Codes");
            //UnityEngine.Object.Destroy(display.transform.Find("BBBoulderMediumRound1").gameObject);
            //UnityEngine.Object.Destroy(display.transform.Find("BBBoulderMediumRound1 (1)").gameObject);
            //UnityEngine.Object.Destroy(display.transform.Find("BBBoulderMediumRound1 (2)").gameObject);
            Transform zone = display.transform.Find("Final Zone");
            zone.localPosition = Vector3.zero;
            var buttonContainer = zone.Find("ButtonContainer");
            for (int i = 1; i < 10; i++)
            {
                var btn = buttonContainer.Find("PortalDialerButton " + i.ToString());
                if (btn)
                {
                    UnityEngine.Object.Destroy(btn.gameObject);
                }
                /*if (!btn)
                {
                    LAILogging.LogWarning("PortalDialerButton " + i.ToString() + " was NOT found.", ConfigSetup.LoggingStyle.UserShouldSee);
                    continue;
                }*/
                //UnityEngine.Object.Destroy(btn.GetComponent<PortalDialerButtonController>());
                //UnityEngine.Object.Destroy(btn.GetComponent<GenericInteraction>());
                //UnityEngine.Object.Destroy(btn.GetComponent<NetworkIdentity>());
                //btn.GetComponent<NetworkIdentity>().enabled = false;
            }
            var dialer = buttonContainer.Find("PortalDialer");
            UnityEngine.Object.Destroy(dialer.GetComponent<PortalDialerController>());
            UnityEngine.Object.Destroy(dialer.GetComponent<NetworkStateMachine>());
            UnityEngine.Object.Destroy(dialer.GetComponent<EntityStateMachine>());
            UnityEngine.Object.Destroy(dialer.GetComponent<PurchaseAvailabilityIndicator>());
            UnityEngine.Object.Destroy(dialer.GetComponent<PurchaseInteraction>());
            UnityEngine.Object.Destroy(dialer.GetComponent<NetworkIdentity>());

            var comp = display.AddComponent<PrefabSpawner>();
            var dialerCont = display.transform.Find("Final Zone/ButtonContainer/PortalDialer").GetComponent<PortalDialerController>();
            comp.localPosition = dialerCont.portalSpawnLocation.position;
            comp.rotation = dialerCont.portalSpawnLocation.rotation;
            comp.enabled = false;
            comp.AssetPath = "RoR2/Base/PortalArtifactworld/PortalArtifactworld.prefab";
        }

        public override void OnVoteStarted(LAIScene scene)
        {
            base.OnVoteStarted(scene);
            if (!scene.IsSceneOfType<Codes>()) return;
            if (!LAISceneManager.sceneInstance) return;
            LAISceneManager.sceneInstance.GetComponent<PrefabSpawner>().enabled = true;
        }
    }
}