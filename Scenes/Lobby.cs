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
            LAIScene.onSceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(LAIScene scene)
        {
            Debug.Log($"Scene: {scene} LayoutInstance: {LAISceneManager.sceneInstance}");
            if (!LAISceneManager.sceneInstance)
                return;
            if (!(LAISceneManager.chosenScene is Lobby))
                return;
            MeshPropsRef = LAISceneManager.sceneInstance.transform.Find("MeshProps").gameObject;

            // Lights //
            HookMethods.Hook_LightUpdate_Color(ConfigSetup.Light_Color.Value);
            HookMethods.Hook_LightUpdate_Flicker(ConfigSetup.Light_Flicker.Value);
            HookMethods.Hook_LightUpdate_Intensity(ConfigSetup.Light_Intensity.Value);

            // Background Elements //
            HookMethods.Hook_HideProps(ConfigSetup.MeshProps.Value);
            HookMethods.Hook_HidePhysicsProps(ConfigSetup.PhysicsProps.Value);
        }
    }
}