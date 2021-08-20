using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class Arena : LAIScene
    {
        public override string SceneName => "Hidden Realm: Void Fields";
        public override GameObject BackgroundPrefab => Resources.Load<GameObject>("prefabs/stagedisplay/ArenaDioramaDisplay");
        public override Vector3 Position => new Vector3(3.5f, -1.5f, 3);
        public override Quaternion Rotation => Quaternion.Euler(0f, 0f, 0);
        public override Vector3 Scale => new Vector3(1f, 0.5f, 1f);
    }
}