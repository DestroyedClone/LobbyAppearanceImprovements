using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class Lobby : LAIScene
    {
        public override string SceneName => "Lobby";
        public override string SceneNameToken => "LAI_MAP_LOBBY";
        public override GameObject BackgroundPrefab => LoadAsset("RoR2/Base/Common/DefaultLobbyBackground.prefab");
        public override Vector3 Position => Vector3.zero;
        public override Quaternion Rotation => Quaternion.identity;
        public override Vector3 Scale => Vector3.one;

        public static string[] PhysicsPropNames = new string[]
        {
            "PropAnchor", "ExtinguisherMesh", "FolderMesh", "LaptopMesh (1)", "ChairPropAnchor", "ChairMesh",
                    "ChairWeight","PropAnchor (1)","ExtinguisherMesh (1)","ExtinguisherMesh (2)", "FolderMesh (1)", "LaptopMesh (2)"
        };

        public static string[] MeshPropNames = new string[]
        {
            "HANDTeaser", "HumanCrate1Mesh", "HumanCrate2Mesh", "HumanCanister1Mesh"
        };

        public static GameObject MeshPropsRef;

        public override void Init()
        {
            base.Init();
            MeshPropsRef = LAISceneManager.sceneInstance.transform.Find("MeshProps").gameObject;
        }
    }
}