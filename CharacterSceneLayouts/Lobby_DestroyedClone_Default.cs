using UnityEngine;

namespace LobbyAppearanceImprovements.CharacterSceneSetups
{
    public class Lobby_DestroyedClone_Default : CharSceneLayout
    {
        public override string SceneLayout => "Lobby_DestroyedClone_Default";
        public override string SceneName => "Lobby";
        public override string Author => "DestroyedClone";
        public override string LayoutName => "Default";

        public override CharacterLayout[] CharacterLayouts => new CharacterLayout[]
        {
            new CharacterLayout
            {
                BodyName = "Commando",
                Position = new Vector3 (2.65f, 0.01f, 6.00f),
                Rotation = new Vector3 (0f, 240f, 0f)
            },
            new CharacterLayout
            {
                BodyName = "Huntress",
                Position = new Vector3 (4.8f, 1.43f, 15.36f),
                Rotation = new Vector3 (-0.21f, 0.15f, 20.84f)
            }
        };
    }
}