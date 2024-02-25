using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class ShipGraveyard : LAIScene
    {
        public override string SceneNameToken => "MAP_SHIPGRAVEYARD";
        public override GameObject BackgroundPrefab => LoadAsset("RoR2/Base/shipgraveyard/ShipgraveyardDioramaDisplay.prefab");
        public override Vector3 Position => new Vector3(5f, -3f, 15);
        public override Quaternion Rotation => Quaternion.Euler(0, 0, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/goldshores/matSkyboxGoldshores.mat");
    }
}