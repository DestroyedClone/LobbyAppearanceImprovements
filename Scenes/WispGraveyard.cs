using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class WispGraveyard : LAIScene
    {
        public override string SceneNameToken => "MAP_WISPGRAVEYARD";
        public override GameObject BackgroundPrefab => LoadAsset("RoR2/Base/wispgraveyard/WispgraveyardDioramaDisplay.prefab");
        public override Vector3 Position => new Vector3(0f, -10.7f, 15);
        public override Quaternion Rotation => Quaternion.Euler(0, 0, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/bazaar/matSkybox4.mat");
    }
}