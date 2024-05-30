﻿using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LobbyAppearanceImprovements.Scenes
{
    public class VoidRaid : LAIScene
    {
        public override string SceneNameToken => "MAP_VOIDRAID";
        public override GameObject BackgroundPrefab => Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/voidraid/VoidRaidDioramaDisplay.prefab").WaitForCompletion();
        public override Vector3 Position => new Vector3(5.5f, -2.8f, 20);
        public override Quaternion Rotation => Quaternion.Euler(340f, 0f, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/DLC1/voidstage/matSkyboxVoidStage.mat");
        public override string MusicTrackName => "muGameplayDLC1_08";
    }
}