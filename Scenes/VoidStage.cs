using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LobbyAppearanceImprovements.Scenes
{
    public class VoidStage : LAIScene
    {
        public override string SceneNameToken => "MAP_VOIDSTAGE";
        public override GameObject BackgroundPrefab => Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/voidstage/VoidStageDiorama.prefab").WaitForCompletion();
        public override Vector3 Position => new Vector3(-44.1364f, -20.5f, 55.4364f);
        public override Quaternion Rotation => Quaternion.Euler(0f, 260f, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
    }
}