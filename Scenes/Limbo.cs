using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class Limbo : LAIScene
    {
        public override string SceneName => "Hidden Realm: A Moment, Whole";
        public override GameObject BackgroundPrefab => Resources.Load<GameObject>("prefabs/stagedisplay/LimboDioramaDisplay");
        public override Vector3 Position => new Vector3(0f, -3.05f, 0);
        public override Quaternion Rotation => Quaternion.Euler(0, 0, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
    }
}