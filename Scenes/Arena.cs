using R2API;
using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class Arena : LAIScene
    {
        public override string SceneNameToken => "MAP_ARENA";
        public override GameObject BackgroundPrefab => displayPrefab;
        public override Vector3 Position => new Vector3(3.5f, - 3.1f, 3);
        public override Quaternion Rotation => Quaternion.Euler(0f, 0f, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/arena/matSkyboxArena.mat");

        public static GameObject displayPrefab;

        public override void Init()
        {
            base.Init();
            displayPrefab = PrefabCloneFromAddressable("RoR2/Base/arena/ArenaDioramaDisplay.prefab", "LAI_ArenaDisplay");
            //displayPrefab.transform.position = Position;
            //displayPrefab.transform.rotation = Rotation;
            //displayPrefab.transform.localScale = Scale;
            var gem = UnityEngine.Object.Instantiate(LAIScene.LoadAsset("RoR2/Base/arena/NullifierGemProp.prefab"), displayPrefab.transform);
            gem.transform.position = new Vector3(3.9f, 0.2f, 8.3f);
            gem.transform.localScale = Vector3.one * 2;
            var portal = Object.Instantiate(LoadAsset("RoR2/Base/PortalArena/PortalArena.prefab"), displayPrefab.transform);
            portal.transform.position = new Vector3(9, 0, 28);
        }
    }
}