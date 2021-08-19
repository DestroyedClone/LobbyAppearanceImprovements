using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class ShipGraveyard : LAIScene
    {
        public override string SceneName => "Ship graveyard";
        public override GameObject BackgroundPrefab => Resources.Load<GameObject>("prefabs/stagedisplay/ShipgraveyardDioramaDisplay");
        public override Vector3 Position => new Vector3(5f, -3f, 15);
        public override Quaternion Rotation => Quaternion.Euler(0, 0, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
    }
}