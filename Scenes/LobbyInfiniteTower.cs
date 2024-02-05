using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class LobbyInfiniteTower : LAIScene
    {
        public override string SceneName => "Voidtouched Lobby";
        public override string SceneNameToken => "LAI_MAP_LOBBY_INFINITETOWER";
        public override GameObject BackgroundPrefab => LoadAsset("RoR2/DLC1/Common/LobbyBackgrounds/InfiniteTowerLobbyBackground.prefab");
        public override Vector3 Position => Vector3.zero;
        public override Quaternion Rotation => Quaternion.identity;
        public override Vector3 Scale => Vector3.one;
    }
}