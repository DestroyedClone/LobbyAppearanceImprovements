using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class BlackBeach : LAIScene
    {
        public override string SceneName => "Distant Roost";
        public override GameObject BackgroundPrefab => Resources.Load<GameObject>("prefabs/stagedisplay/BlackbeachDioramaDisplay");
        public override Vector3 Position => new Vector3(0f, -2.4f, 5);
        public override Quaternion Rotation => new Quaternion();
        public override Vector3 Scale => new Vector3(1f, 0.5f, 1f);
    }
}