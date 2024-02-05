using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LobbyAppearanceImprovements.Scenes
{
    public class SnowyForest : LAIScene
    {
        public override string SceneName => "Siphoned Forest";
        public override string SceneNameToken => "MAP_SNOWYFOREST";
        public override GameObject BackgroundPrefab => Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/snowyforest/SnowyForestDioramaDisplay.prefab").WaitForCompletion();
        public override Vector3 Position => new Vector3(3.5f, -4.6f, 21);
        public override Quaternion Rotation => Quaternion.Euler(0f, 210f, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
    }
}