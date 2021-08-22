using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class CaptainsHelm : LAIScene
    {
        public override string SceneName => "Helm of the Ship";
        public override GameObject BackgroundPrefab => Resources.Load<GameObject>("prefabs/stagedisplay/ArenaDioramaDisplay");
        public override Vector3 Position => new Vector3(0, -0, 0);
        public override Quaternion Rotation => Quaternion.Euler(0f, 0f, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);


    }
}