using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LobbyAppearanceImprovements.Scenes
{
    public class SulfurPools : LAIScene
    {
        public override string SceneNameToken => "MAP_SULFURPOOLS";
        public override GameObject BackgroundPrefab => Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/sulfurpools/SulfurpoolsDioramaDisplay.prefab").WaitForCompletion();
        public override Vector3 Position => new Vector3(3.5f, -6f, 20f);
        public override Quaternion Rotation => Quaternion.Euler(0f, 0f, 0);
        public override Vector3 Scale => new Vector3(1f, 0.5f, 1f);
    }
}