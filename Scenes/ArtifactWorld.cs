using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class ArtifactWorld : LAIScene
    {
        public override string SceneName => "Hidden Realm: Bulwark's Ambry";
        public override string SceneNameToken => "MAP_ARTIFACTWORLD";
        public override GameObject BackgroundPrefab => LoadAsset("RoR2/Base/artifactworld/ArtifactworldDioramaDisplay.prefab");
        public override Vector3 Position => new Vector3(4f, -2.5f, 0);
        public override Quaternion Rotation => Quaternion.Euler(0, 270, 0);
        public override Vector3 Scale => new Vector3(1f, 0.5f, 1f);
    }
}