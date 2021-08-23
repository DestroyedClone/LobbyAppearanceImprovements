using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class Moon : LAIScene
    {
        public override string SceneName => "Commencement";
        public override GameObject BackgroundPrefab => Resources.Load<GameObject>("prefabs/stagedisplay/MoonDioramaDissplay");
        public override Vector3 Position => new Vector3(5, -5, 20);
        public override Quaternion Rotation => Quaternion.Euler(0f, 0f, 0);
        public override Vector3 Scale => new Vector3(1, 0.1f, 1);
    }
}