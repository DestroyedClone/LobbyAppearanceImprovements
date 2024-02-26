using R2API;
using System;
using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class ArtifactWorld : LAIScene
    {
        public override string SceneNameToken => "MAP_ARTIFACTWORLD";
        public override GameObject BackgroundPrefab => display;
        public override Vector3 Position => new Vector3(4f, -2.5f, 0);
        public override Quaternion Rotation => Quaternion.Euler(0, 270, 0);
        public override Vector3 Scale => new Vector3(1f, 0.5f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/artifactworld/matSkyboxArtifactWorld.mat");

        public static GameObject display { get; private set; }

        public override void Init()
        {
            base.Init();
            display = PrefabCloneFromAddressable("RoR2/Base/artifactworld/ArtifactworldDioramaDisplay.prefab", "LAI_Scene_ArtifactWorld");

            //var portal = CloneFromAddressable("RoR2/Base/PortalArtifactworld/PortalArtifactworld.prefab", DisplayPrefab.transform);
            //portal.transform.position = new Vector3(0, 4, 23);
            var portal = display.AddComponent<PrefabSpawner>();
            portal.AssetPath = "RoR2/Base/PortalArtifactworld/PortalArtifactworld.prefab";
            portal.position = new Vector3(0, 4, 23);

            var skybox = CloneFromAddressable("RoR2/Base/artifactworld/ArtifactWorldSkybox.prefab", display.transform);
        }
    }
}