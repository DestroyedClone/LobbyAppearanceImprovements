using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class DampCave : LAIScene
    {
        public override string SceneName => "Damp Cave";
        public override GameObject BackgroundPrefab => Resources.Load<GameObject>("prefabs/stagedisplay/BlackbeachDioramaDisplay");
        public override Vector3 Position => new Vector3(4f, -2f, 4);
        public override Quaternion Rotation => Quaternion.Euler(0, 60, 0);
        public override Vector3 Scale => new Vector3(1f, 0.5f, 1f);
    }
}