using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LobbyAppearanceImprovements.CharacterSceneLayouts
{
    public class Lobby_DestroyedClone_Original : CharSceneLayout
    {
        public override string SceneLayout => "Lobby_DestroyedClone_Original";
        public override string SceneName => "Lobby";
        public override string Author => "DestroyedClone";
        public override string LayoutName => "Default";
        public override Dictionary<string, Vector3[]> CharacterLayouts => new Dictionary<string, Vector3[]>()
        {

        };

        public override CharacterLayouzt[] CharacterLayouts => new CharacterLayout[]
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
                Position = new Vector3 (2.33f, 0.01f, 7.4f),
                Rotation = new Vector3 (0f, 240f, 0f)
            },
            new CharacterLayout
            {
                BodyName = "Bandit2",
                Position = new Vector3 (3.79f, 0.01f, 11.5f),
                Rotation = new Vector3 (0f, 240f, 0f)
            },
            new CharacterLayout
            {
                BodyName = "Engi",
                Position = new Vector3 (0.2f, 0f, 20.4f),
                Rotation = new Vector3 (0f, 240f, 0f)
            },
            new CharacterLayout
            {
                BodyName = "Merc",
                Position = new Vector3 (4.16f, 1.3f, 17f),
                Rotation = new Vector3 (0f, 240f, 0f)
            },
            new CharacterLayout
            {
                BodyName = "Loader",
                Position = new Vector3 (-4f, 0f, 20.22f),
                Rotation = new Vector3 (0f, 140f, 0f)
            },
            new CharacterLayout
            {
                BodyName = "Croco",
                Position = new Vector3 (-4f, 0f, 13f),
                Rotation = new Vector3 (0f, 120f, 0f)
            },
        };
    }
}
