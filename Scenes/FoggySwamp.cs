using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class FoggySwamp : LAIScene
    {
        public override string SceneName => "Wetland Aspect";
        public override string SceneNameToken => "MAP_FOGGYSWAMP";
        public override GameObject BackgroundPrefab => LoadAsset("RoR2/Base/foggyswamp/FoggyswampDioramaDisplay.prefab");
        public override Vector3 Position => new Vector3(0f, -2.7f, 16);
        public override Quaternion Rotation => Quaternion.Euler(0, 30, 0);
        public override Vector3 Scale => new Vector3(1f, 0.5f, 1f);
    }
}