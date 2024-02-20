using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class LobbyVoid : LAIScene
    {
        public override string SceneNameToken => "LAI_MAP_LOBBY_INFINITETOWER";
        public override GameObject BackgroundPrefab => LoadAsset("RoR2/DLC1/Common/LobbyBackgrounds/InfiniteTowerLobbyBackground.prefab");
        public override Vector3 Position => Vector3.zero;
        public override Quaternion Rotation => Quaternion.identity;
        public override Vector3 Scale => Vector3.one;
    }
}