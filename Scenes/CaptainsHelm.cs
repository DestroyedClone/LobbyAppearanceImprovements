using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class CaptainsHelm : LAIScene
    {
        public override string SceneName => "Helm of the Ship/Bridge";
        public override GameObject BackgroundPrefab => LAIPlugin.CaptainHelmObject;
        public override Vector3 Position => new Vector3(-16.55f, -3.25f, 0);
        public override Quaternion Rotation => Quaternion.Euler(0f, 180f, 0);
        public override Vector3 Scale => new Vector3(50f, 50f, 50f);


    }
}