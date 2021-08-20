using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class GoldShores : LAIScene
    {
        public override string SceneName => "Hidden Realm: Gilded Coast";
        public override GameObject BackgroundPrefab => Resources.Load<GameObject>("prefabs/stagedisplay/GoldshoresDiorama");
        public override Vector3 Position => new Vector3(0f, -2.7f, 16);
        public override Quaternion Rotation => Quaternion.Euler(0, 30, 0);
        public override Vector3 Scale => new Vector3(1f, 0.5f, 1f);
    }
}