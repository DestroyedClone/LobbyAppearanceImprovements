using R2API;
using System;
using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class ArtifactWorld : LAIScene
    {
        public override string SceneNameToken => "MAP_ARTIFACTWORLD";
        public override GameObject BackgroundPrefab => DisplayPrefab;
        public override Vector3 Position => new Vector3(4f, -2.5f, 0);
        public override Quaternion Rotation => Quaternion.Euler(0, 270, 0);
        public override Vector3 Scale => new Vector3(1f, 0.5f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/artifactworld/matSkyboxArtifactWorld.mat");

        public static GameObject DisplayPrefab { get; private set; }

        public override void Init()
        {
            base.Init();
            DisplayPrefab = PrefabAPI.InstantiateClone(LoadAsset("RoR2/Base/artifactworld/ArtifactworldDioramaDisplay.prefab"), "LAI_ArtifactWorldDisplay", false);

            var portal = UnityEngine.Object.Instantiate(LoadAsset("RoR2/Base/PortalArtifactworld/PortalArtifactworld.prefab"), DisplayPrefab.transform);
            portal.transform.position = new Vector3(0, 4, 23);
        }
    }
}