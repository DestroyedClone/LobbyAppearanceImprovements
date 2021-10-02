using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class GooLake : LAIScene
    {
        public override string SceneName => "Abandoned Aqueduct";
        public override GameObject BackgroundPrefab => Resources.Load<GameObject>("prefabs/stagedisplay/GoolakeDioramaDisplay");
        public override Vector3 Position => new Vector3(0, -5f, 8);
        public override Quaternion Rotation => Quaternion.Euler(0, 0, 0);
        public override Vector3 Scale => Vector3.one;
    }
}