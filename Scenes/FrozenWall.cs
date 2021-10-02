using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class FrozenWall : LAIScene
    {
        public override string SceneName => "Rallypoint Delta";
        public override GameObject BackgroundPrefab => Resources.Load<GameObject>("prefabs/stagedisplay/FrozenwallDioramaDisplay");
        public override Vector3 Position => new Vector3(7f, -2.25f, 7);
        public override Quaternion Rotation => Quaternion.Euler(0, 260, 0);
        public override Vector3 Scale => new Vector3(.5f, 0.5f, .5f);
    }
}