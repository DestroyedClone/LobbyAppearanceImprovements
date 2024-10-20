﻿using RoR2;
using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class ArtifactWorld01 : LAIScene
    {
        public override string SceneNameToken => "MAP_ARTIFACTWORLD01";
        public override string SeerToken => "LAI_SEER_ARTIFACTWORLD";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new Vector3(-2, - 6, 14);
        public override Quaternion Rotation => Quaternion.Euler(0, 90, 0);
        //public override Vector3 Position => new Vector3(4f, -2.5f, 0);
        //public override Quaternion Rotation => Quaternion.Euler(0, 270, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/artifactworld/matSkyboxArtifactWorld.mat");
        public override string MusicTrackName => "muSong13";

        public static GameObject display { get; private set; }


        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/DLC2/artifactworld01/Artifactworld01DioramaDisplay.prefab", "LAI_Scene_ArtifactWorld");

            //var portal = CloneFromAddressable("RoR2/Base/PortalArtifactworld/PortalArtifactworld.prefab", DisplayPrefab.transform);
            //portal.transform.position = new Vector3(0, 4, 23);
            var portal = display.AddComponent<PrefabSpawner>();
            portal.AssetPath = "RoR2/Base/PortalArtifactworld/PortalArtifactworld.prefab";
            //portal.position = new Vector3(0f, 5f, 30f);
            //portal.localPosition = new Vector3(26, 16, 4); //annoying
            //portal.localPosition = new Vector3(-46, 17, 4); //??????????????????
            portal.localPosition = new Vector3(-20, 16, 5);
            portal.rotation = Quaternion.Euler(0, 270, 0);
            portal.enabled = false;

            var skybox = CloneFromAddressable("RoR2/Base/artifactworld/ArtifactWorldSkybox.prefab", display.transform);
        }

        public override void OnVoteStarted(LAIScene scene)
        {
            base.OnVoteStarted(scene);
            if (!scene.IsSceneOfType<Arena>()) return;
            var sceneInstance = LAISceneManager.sceneInstance;
            if (!sceneInstance) return;
            sceneInstance.GetComponent<PrefabSpawner>().enabled = true;
        }
    }
}