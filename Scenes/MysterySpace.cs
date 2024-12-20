﻿using RoR2;
using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class MysterySpace : LAIScene
    {
        public override string SceneNameToken => "MAP_MYSTERYSPACE";
        public override string SeerToken => "LAI_SEER_MYSTERYSPACE";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new(0f, -5f, 30);
        public override Quaternion Rotation => Quaternion.Euler(0, 345, 0);
        public override Vector3 Scale => new(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/mysteryspace/matSkyboxMysterySpace.mat");
        public override string MusicTrackName => "muSong21";
        public static GameObject display;

        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/Base/mysteryspace/MysteryspaceDioramaDisplay.prefab", "LAI_Scene_MysterySpace");
            var obelisk = display.transform.Find("Ring/Ruins/MSObelisk");
            UnityEngine.Object.Destroy(obelisk.GetComponent<PurchaseInteraction>());

            CloneFromAddressable("RoR2/Base/mysteryspace/MSSkybox.prefab", display.transform);
            //has shakeemitter on it
        }

        public override void OnVoteStarted(LAIScene scene)
        {
            //if (!(scene is MysterySpace)) return;
            if (!scene.IsSceneOfType<MysterySpace>()) return;
            var sceneInstance = LAISceneManager.sceneInstance;
            if (!LAISceneManager.sceneInstance) return;
            sceneInstance.transform.Find("Ring/Ruins/MSObelisk/Stage1FX/").gameObject.SetActive(true);
        } //disable shake emitter
    }
}