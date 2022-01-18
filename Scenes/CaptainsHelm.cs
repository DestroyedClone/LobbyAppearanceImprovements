using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class CaptainsHelm : LAIScene
    {
        public override string SceneName => "Helm of the Ship/Bridge";
        public override GameObject BackgroundPrefab => SceneSetup.CaptainHelmObject;
        public override Vector3 Position => new Vector3(-16.7f, -2f, 8);
        public override Quaternion Rotation => Quaternion.Euler(10f, 179f, 0);
        public override Vector3 Scale => new Vector3(50f, 50f, 50f);
        public override string PreferredLayout => nameof(CharacterSceneLayouts.Any_Empty);

    }
}