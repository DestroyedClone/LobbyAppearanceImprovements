using LobbyAppearanceImprovements.Scenes;
using System.Collections.Generic;
using UnityEngine;

namespace LobbyAppearanceImprovements.Layouts
{
    public class Moon_Default : LAILayout
    {
        public override string SceneLayout => "Moon_Default";
        public override string SceneName => "Moon";
        public override string Author => "DestroyedClone";
        public override string ReadmeDescription => "";
        public override string LayoutNameToken => "LAI_LAYOUT_MOONDEFAULT";

        public override Dictionary<string, Vector3[]> CharacterLayouts => new Dictionary<string, Vector3[]>()
        {
            //{ "Commando", new [] {new Vector3(2.65f, 0.01f, 6.00f), new Vector3(0f, 240f, 0f) } },
        };

        public override Dictionary<string, CameraSetting> CharacterCameraSettings => new Dictionary<string, CameraSetting>()
        {
            { "CommandoBody", new CameraSetting(60f, new Vector3(0.89f, 2.3f, 14.35f), new Vector3(2.1f, 340.7f, 0)) }
        };

        public override List<GameObject> CreateAdditionalObjectsOnLoad()
        {
            List<GameObject> list = new List<GameObject>();

            var brother = LAIScene.CloneFromAddressable("RoR2/DLC1/itmoon/mdlBrotherConstellation.prefab");
            brother.transform.position = new Vector3(0, -790, 900);
            brother.transform.rotation = Quaternion.Euler(0, 170, 0);
            brother.transform.localScale = Vector3.one * 500;
            list.Add(brother);

            var com = Object.Instantiate(UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>("RoR2/Junk/golemplains_trailer/DeadCommandoProp.prefab").WaitForCompletion());
            com.transform.position = new Vector3(-2, 0.4f, 21.9f);
            list.Add(com);

            return list;
        }
    }
}