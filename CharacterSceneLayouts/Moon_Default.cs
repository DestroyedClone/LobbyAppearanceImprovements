using System.Collections.Generic;
using UnityEngine;

namespace LobbyAppearanceImprovements.CharacterSceneLayouts
{
    public class Moon_Default : CharSceneLayout
    {
        public override string SceneLayout => "Moon_Default";
        public override string SceneName => "Moon";
        public override string Author => "DestroyedClone";
        public override string ReadmeDescription => "";
        public override string LayoutNameToken => "LAI_LAYOUT_MOONDEFAULT";

        public override Dictionary<string, Vector3[]> CharacterLayouts => new Dictionary<string, Vector3[]>()
        {
            { "Commando", new [] {new Vector3(2.65f, 0.01f, 6.00f), new Vector3(0f, 240f, 0f) } },
        };

        public override List<GameObject> CreateAdditionalObjectsOnLoad()
        {
            List<GameObject> list = new List<GameObject>();

            var brother = UnityEngine.Object.Instantiate(SceneSetup.brotherConstellation);
            brother.transform.position = new Vector3(0, - 790, 900);
            brother.transform.rotation = Quaternion.Euler(0, 170, 0);
            brother.transform.localScale = Vector3.one * 500;
            list.Add(brother);

            return list;
        }
    }
}