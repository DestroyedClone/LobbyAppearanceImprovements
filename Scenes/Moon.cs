using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class Moon : LAIScene
    {
        public override string SceneName => "Commencement";
        public override string SceneNameToken => "MAP_MOON";
        public override GameObject BackgroundPrefab => LoadAsset("RoR2/Base/moon/MoonDioramaDissplay.prefab");
        public override Vector3 Position => new Vector3(0, -4.6f, 25);
        public override Quaternion Rotation => Quaternion.Euler(0f, 90f, 0);
        public override Vector3 Scale => new Vector3(1, 1f, 1);
        public static GameObject MoonDioramaFinal = null;
    }
}