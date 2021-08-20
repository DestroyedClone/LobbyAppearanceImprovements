using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class WispGraveyard : LAIScene
    {
        public override string SceneName => "Scorched Acres";
        public override GameObject BackgroundPrefab => Resources.Load<GameObject>("prefabs/stagedisplay/WispgraveyardDioramaDisplay");
        public override Vector3 Position => new Vector3(0f, -10.7f, 15);
        public override Quaternion Rotation => Quaternion.Euler(0, 0, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
    }
}