using R2API;
using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class Arena : LAIScene
    {
        public override string SceneNameToken => "MAP_ARENA";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new Vector3(3.5f, - 3.1f, 3);
        public override Quaternion Rotation => Quaternion.Euler(0f, 0f, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/arena/matSkyboxArena.mat");

        public static GameObject display;

        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/Base/arena/ArenaDioramaDisplay.prefab", "LAI_Scene_Arena");
            //displayPrefab.transform.position = Position;
            //displayPrefab.transform.rotation = Rotation;
            //displayPrefab.transform.localScale = Scale;
            var gem = CloneFromAddressable("RoR2/Base/arena/NullifierGemProp.prefab", display.transform);
            gem.transform.position = new Vector3(3.9f, 0.2f, 8.3f);
            gem.transform.localScale = Vector3.one * 2;
            //var portal = CloneFromAddressable("RoR2/Base/PortalArena/PortalArena.prefab", display.transform);
            //portal.transform.position = new Vector3(9, 0, 28);
            var portal = display.AddComponent<PrefabSpawner>();
            portal.AssetPath = "RoR2/Base/PortalArena/PortalArena.prefab";
            portal.position = new Vector3(9, 0, 28);

            var skybox = CloneFromAddressable("RoR2/Base/arena/ArenaSkybox.prefab");
            skybox.transform.parent = display.transform;
        }
    }
}