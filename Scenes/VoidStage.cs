using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LobbyAppearanceImprovements.Scenes
{
    public class VoidStage : LAIScene
    {
        public override string SceneName => "Hidden Realm: Void Fields";
        public override string SceneNameToken => "MAP_VOIDSTAGE";
        public override GameObject BackgroundPrefab => Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/voidstage/VoidStageDiorama.prefab").WaitForCompletion();
        public override Vector3 Position => new Vector3(3.5f, -1.5f, 3);
        public override Quaternion Rotation => Quaternion.Euler(0f, 0f, 0);
        public override Vector3 Scale => new Vector3(1f, 0.5f, 1f);
    }
}