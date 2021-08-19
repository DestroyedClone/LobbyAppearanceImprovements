using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class GolemPlains : LAIScene
    {
        public override string SceneName => "GolemPlains";
        public override GameObject BackgroundPrefab => Resources.Load<GameObject>("prefabs/stagedisplay/GolemPlainsDioramaDisplay");
        public override Vector3 Position => new Vector3(9f, -3.1f, 9);
        public override Quaternion Rotation => Quaternion.Euler(0, 180, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
    }
}