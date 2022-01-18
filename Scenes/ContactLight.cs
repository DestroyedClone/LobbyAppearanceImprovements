using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class ContactLight : LAIScene
    {
        public override string SceneName => "UES Contact Light";
        public override GameObject BackgroundPrefab => SceneSetup.CaptainHelmObject;
        public override Vector3 Position => new Vector3(0f, 0f, 0);
        public override Quaternion Rotation => Quaternion.Euler(0f, 0f, 0);
        public override Vector3 Scale => new Vector3(0f, 0f, 0f);
        public override string PreferredLayout => nameof(CharacterSceneLayouts.Any_Empty);

    }
}