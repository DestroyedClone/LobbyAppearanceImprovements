using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class SkyMeadow : LAIScene
    {
        public override string SceneName => "Sky Meadow";
        public override GameObject BackgroundPrefab => Resources.Load<GameObject>("prefabs/stagedisplay/SkyMeadowDioramaDisplay");
        public override Vector3 Position => new Vector3(0f, -3f, 0);
        public override Quaternion Rotation => Quaternion.Euler(0, 0, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
    }
}