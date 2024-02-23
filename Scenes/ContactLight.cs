using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class ContactLight : LAIScene
    {
        public override string SceneNameToken => "LAI_MAP_CONTACTLIGHT";
        public override GameObject BackgroundPrefab => SceneSetup.ContactLight_Bandit2;
        public override Vector3 Position => new Vector3(0f, 0f, 3);
        public override Quaternion Rotation => Quaternion.Euler(0f, 180f, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override string PreferredLayout => nameof(Layouts.Any_Empty);
    }
}